using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public static int LayerFloor;

    protected SpriteRenderer sRenderer;
    protected Rigidbody2D rb;
    protected BoxCollider2D col;
    protected Animator anim;

    protected bool onFloor = true;
    protected Pose myPose;
    public string myItem;
    public bool HasMirror = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        LayerFloor = LayerMask.GetMask("Floor");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // Check if on floor
        Collider2D[] hitColliders = new Collider2D[6];
        int hitNum = Physics2D.OverlapCircleNonAlloc(new Vector2(transform.position.x, transform.position.y - 0.7f), 0.33f, hitColliders, LayerFloor);
        if (hitNum > 0 != onFloor)
            onFloor = hitNum > 0;

        HandlePose();
    }

    public virtual void CrouchOn()
    {
        col.size = new Vector2(0.7f, 0.86f);
        col.offset = new Vector2(0f, -0.55f);
        anim.SetBool("Crouching", true);
    }

    public virtual void CrouchOff()
    {
        col.size = new Vector2(0.7f, 1.736f);
        col.offset = new Vector2(0f, -0.111f);
        anim.SetBool("Crouching", false);
    }
    

    public virtual void HandlePose()
    {
        // JUMPING - - - - - - - - - - - - - - - -
        if (!onFloor && myPose != Pose.Jumping)
        {
            anim.SetBool("Falling", true);
            anim.SetBool("Walking", false);
            SetPose(Pose.Jumping);
        }

        if (myPose == Pose.Jumping && onFloor)
        {
            anim.SetBool("Falling", false);
            SetPose(Pose.Idle);
        }


        // WALKING - - - - - - - - - - - - - - - -
        bool moving = rb.velocity.x != 0;

        if (moving && (myPose == Pose.Idle || myPose == Pose.Crouching))
        {
            anim.SetBool("Walking", true);
            SetPose(Pose.Walking);
        }
        if (!moving && (myPose == Pose.Walking))
        {
            anim.SetBool("Walking", false);
            SetPose(Pose.Idle);
        }
    }

    public virtual void DeflectLaser()
    {
        anim.SetTrigger("DeflectLaser");
        ItemController.AllItems[myItem].Deflect();
    }

    public virtual void Vaporize()
    {
        anim.SetTrigger("DIE");
    }

    public virtual void DropItem()
    {
        ItemController.AllItems[myItem].PutDown(transform);
        SetItem("Empty");
        anim.SetBool("Carrying", false);
    }

    public virtual void SetPose(Pose newPose)
    {
        myPose = newPose;
    }

    public virtual void SetItem(string newItem)
    {
        myItem = newItem;
        if (newItem.Contains("Mirror") != HasMirror)
            HasMirror = newItem.Contains("Mirror");
    }
}
