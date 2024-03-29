using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Person
{
    public static Vector2 MyPosition;
    public static bool MyDirection;
    public static Pose MyPose;
    public static string MyItem = "Empty";
    public static PlayerController Instance;
    public static int PushState = 0;
    // interpretation:
    // 0 - not trying to push anything
    // 1 - ready to begin pushing
    // 2 - pushing to the right
    // 3 - pushing to the left

    public static bool MovementEnabled = true;


    // Start is called before the first frame update
    public override void Start()
    {
        Instance = this;
        EventManager.ResetLevel += Replace;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (MovementEnabled)
        {
            Move();
            if (Input.GetMouseButtonDown(0) && PushState == 0)
            {
                if (!Interact())
                    PushState = 1;
            }

            if (Input.GetMouseButtonUp(0) && PushState > 0)
                PushState = 0;
        }
    }

    // Summary:
    //     Called every Update when movement is enabled. Handles controls and records for walking, jumping,
    //     crouching, and poses. 
    void Move()
    {
        // Looking & moving
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow))
            UpdateDirection(false);
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow))
            UpdateDirection(true);
        if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            UpdateDirection(true);
        if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            UpdateDirection(false);

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            float deltaX = Input.GetAxis("Horizontal") * (onFloor ? 4f : 2f);
            rb.velocity = new Vector2(deltaX, rb.velocity.y);

            // Jumping
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && onFloor && PushState < 2)
            {
                anim.SetTrigger("Jump");
                rb.AddForce(new Vector2(0, 14000));
            }
        }

        // Crouching
        HandleCrouching();
        
        // Log position and pose
        MyPosition = new Vector2(transform.position.x, transform.position.y);
        HandlePose();
    }

    void UpdateDirection(bool dir)
    {
        if (dir == MyDirection)
            return;
        transform.eulerAngles = new Vector3(0, dir ? 0 : 180, 0);
        MyDirection = dir;
    }

    // Summary:
    //     Called when LeftClick is pressed and movement is enabled. Makes Player drop current held object if they
    //     are carrying one. If they are not, searches for nearby pickuppable objects and picks one up.
    bool Interact()
    {
        Collider2D[] objs = new Collider2D[3];
        int objNum = Physics2D.OverlapCircleNonAlloc(transform.position, 0.35f, objs, Item.LayerCarry);

        // Drop objects
        if (MyItem != "Empty")
        {
            DropItem();
            return true;
        }

        if (objNum == 0)
        {
            return false;
        }

        // Pick up objects
        foreach (Collider2D c in objs)
        {
            CarryableItem item = c.GetComponent<CarryableItem>();
            if (!item.IsHeld && item.Pickuppable)
            {
                SetItem(item.PickUp(transform));
                anim.SetBool("Carrying", true);
                return true;
            }
        }

        return false;
    }

    // Summary:
    //     Called every Update (via Move()) when movement is enabled. Changes Player's
    //     sprite and collider size when the shift button is pressed or depressed.
    void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && PushState < 2)
        {
            if (MyItem != "Empty")
                DropItem();
            SetPose(Pose.Crouching);
            rb.velocity = Vector3.zero;
            base.CrouchOn();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift) && PushState < 2)
        {
            SetPose(Pose.Idle);
            base.CrouchOff();
        }
    }

    public override void DeflectLaser()
    {
        base.DeflectLaser();
        rb.AddForce(new Vector2(2000f, 200f));
    }

    public override void Vaporize()
    {
        base.Vaporize();
        EventManager.PlayerDeath?.Invoke();
        MovementEnabled = false;
        rb.velocity = Vector2.zero;
    }

    /*public override bool StartPushing(string item, bool dir)
    {
        if (MyItem != "Empty" || Input.GetKey(KeyCode.LeftShift))
            return false;
        
        PushState = dir ? 3 : 2;
        MyItem = item;
        return true;
    }*/

    public override void SetPose(Pose newPose)
    {
        base.SetPose(newPose);
        MyPose = newPose; // Log in a static var for the recorder to find
    }

    public override void SetItem(string newItem)
    {
        base.SetItem(newItem);
        MyItem = newItem; // Log in a static var for the recorder to find
    }

    // Summary:
    //     Resets Player when the level resets (after a death). 
    public void Replace()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("Crouching", false);
        anim.SetBool("Carrying", false);
        anim.SetBool("Falling", false);
        anim.Play("Idle", 0, 0);
        MovementEnabled = true;
        transform.position = new Vector2(-22.84f, -2f);
    }

    // Summary:
    //     Condenses information about Player's position, direction, pose, and held item into
    //     a State object for RecordingManager to save. 
    //
    // Parameters:
    //   lastItem:
    //     The last logged item in the recording (to check for pick-up events).
    //   out updated:
    //     Signifies whether there was a pick-up event.
    // Returns:
    //     The current State of the Player.
    public static State GetState(string lastItem, out bool updated)
    {
        if (lastItem != MyItem)
        {
            updated = true;
            return new State(MyPosition, MyDirection, Pose.Using, MyItem);
        }
        else
        {
            updated = false;
            return new State(MyPosition, MyDirection, MyPose, MyItem);
        }
    }
}

public enum Pose
{
    Walking,
    Idle,
    Jumping,
    Crouching,
    Using,
    Pushing
}