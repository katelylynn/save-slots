using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoController : Person
{
    State[] script = new State[0];
    float dur = 1f / RecordingManager.TicksPerSecond;
    float _t;
    
    int scriptTick = 0;
    bool active = false;
    Vector3 faceLeft = new Vector3(0, 180, 0);
    Pose pastPose;
    string pastItem;
    GameObject crouchCollider;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        crouchCollider = transform.Find("Collider").gameObject;
    }

    public void BeginLife(State[] scriptIn)
    {
        script = scriptIn;
        active = true;
    }

    // Summary:
    //     Puppeteers the Echo based off of the script it is following.
    //     base.Update() should NOT be called, as it is not relevant to Echos.
    public override void Update()
    {
        if (active)
        {
            _t += Time.deltaTime;
            int max = 3;
            while(_t > dur && max > 0)
            {
                _t -= dur;
                max--;
                TickScript();
            }
        }
    }

    // Summary:
    //     Called every script tick (via base.TickScript()). Reads the Pose states logged in the script
    //     and sets Echo's animations accordingly.
    public override void HandlePose()
    {
        if (myPose == Pose.Using)
        {
            if (myItem != "Empty") // PICK UP - - - - -
            {
                Collider2D[] objs = new Collider2D[3];
                int objNum = Physics2D.OverlapCircleNonAlloc(transform.position, 0.35f, objs, ItemController.LayerItem);
                
                if (objNum > 0)
                {
                    foreach (Collider2D c in objs)
                    {
                        ItemController item = c.GetComponent<ItemController>();
                        if (!item.IsHeld && item.Pickuppable && item.Item == myItem)
                        {
                            SetItem(item.PickUp(transform));
                            anim.SetBool("Carrying", true);
                            pastItem = myItem;
                            return;
                        }
                    }
                }
            }
            else if (pastItem != "Empty") // DROP - - - -
            {
                ItemController.AllItems[pastItem].PutDown(transform);
                pastItem = "Empty";
                anim.SetBool("Carrying", false);
            }

            return;
        }

        // JUMPING - - - - - - - - - - - - - - - -
        if (myPose == Pose.Jumping)
        {
            if (pastPose != Pose.Jumping)
                anim.SetTrigger("Jump");
            anim.SetBool("Falling", true);
            anim.SetBool("Walking", false);

            pastPose = myPose;
            return;
        }
        else
        {
            anim.SetBool("Falling", false);
        }

        // CROUCHING - - - - - - - - - - - - - - - -
        if (myPose == Pose.Crouching)
        {
            anim.SetBool("Walking", false);
            base.CrouchOn();
            crouchCollider.SetActive(true);

            pastPose = myPose;
            return;
        }
        else
        {
            anim.SetBool("Crouching", false);
            if (pastPose == Pose.Crouching)
            {
                base.CrouchOff();
                crouchCollider.SetActive(false);
            }
        }


        // WALKING - - - - - - - - - - - - - - - -
        if (myPose == Pose.Walking)
        {
            anim.SetBool("Walking", true);

            pastPose = myPose;
            return;
        }

        // IDLE - - - - - - - - - - - - - - - - - -
        if (myPose == Pose.Idle)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Crouching", false);

            pastPose = myPose;
        }
    }

    public override void Vaporize()
    {
        base.Vaporize();
        Destroy(gameObject, 0.4f);
    }

    // Summary:
    //     Called every script tick (via Update()). Handles the Echo's imitation of Player's
    //     past life by reading a state once every certain interval.
    void TickScript()
    {
        transform.position = script[scriptTick].Position;
        transform.eulerAngles = script[scriptTick].Direction ? Vector3.zero : faceLeft;
        if (myItem != script[scriptTick].Item)
            SetItem(script[scriptTick].Item);
        if (myPose != script[scriptTick].Pose)
        {
            SetPose(script[scriptTick].Pose);
            HandlePose();
        }
        scriptTick++;

        if (scriptTick >= script.Length)
        {
            active = false;
            Vaporize();
        }
    }
}
