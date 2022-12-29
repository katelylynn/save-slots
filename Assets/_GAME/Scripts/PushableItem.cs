using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableItem : Item
{
    bool IsMoving = false;

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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Begin pushing
            if (PlayerController.PushState == 1 && collision.gameObject.GetComponent<PlayerController>().StartPushing(ItemId, collision.transform.position.x <= transform.position.x))
            {
                IsMoving = true;
            }
        }
    }
}
