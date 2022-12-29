using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableItem : Item
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.transform.tag == "Person")
        {
            // If crouching, stop
            

            // Begin pushin
            pushing = (byte)(collision.transform.position.x >= transform.position.x ? 2 : 3);
            StartPushing(collision.transform.GetComponent<PushableObject>());
        }*/
    }
}
