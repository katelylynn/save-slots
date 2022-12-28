using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableItem : Item
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        transform.tag = "Pushable";
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (pushing == 1 && collision.transform.tag == "Pushable")
        {
            Debug.Log("Pushing");

            // If crouching, stop
            SetPose(Pose.Idle);
            base.CrouchOff();

            // Begin pushin
            pushing = (byte)(collision.transform.position.x >= transform.position.x ? 2 : 3);
            StartPushing(collision.transform.GetComponent<PushableObject>());
        }
    }*/
}
