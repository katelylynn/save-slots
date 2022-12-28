using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public int Step = 0;
    GameObject p_FlashEffect;

    // Start is called before the first frame update
    void Start()
    {
        p_FlashEffect = Resources.Load<GameObject>("Prefabs/Flash");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.right * Time.deltaTime * 20f);
    }

    // Summary:
    //     Detects collisions with Persons or Mirrors.
    //
    // Parameters:
    //   collision:
    //     The object with which the laser collides.
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision Detected! Step: " + Step);

        if (collision.transform.tag == "Person")
        {
            Person p = collision.transform.GetComponent<Person>();
            if (p.hasMirror && collision.transform.eulerAngles.y == 0 && Step % 2 == 0)
            {
                p.DeflectLaser();
                Deflect();
            }
            else
            {
                p.Vaporize();
            }

            return;
        }

        if (collision.transform.tag == "Reflector" && Step == 1)
        {
            Deflect();
        }
    }

    // Summary:
    //     Spawns a quick flash sprite when the laser reflects off of a mirror.
    //
    // Parameters:
    //   pos:
    //     The position to spawn the flash effect.
    void FlashEffect(Vector2 pos)
    {
        GameObject f = Instantiate(p_FlashEffect, pos, Quaternion.identity).gameObject;
        Destroy(f, 0.4f);
    }

    // Summary:
    //     Handles deflection of the laser off of a mirror. The transformation the laser experiences depends
    //     on how many deflections it has undergone already.
    void Deflect()
    {
        FlashEffect(transform.position + transform.TransformDirection(Vector3.right * -1.5f));
        switch (Step)
        {
            case 0:
                transform.Translate(new Vector2(-1f, 0.5f));
                transform.eulerAngles = new Vector3(0, 0, 200f);
                break;
            case 1:
                transform.Translate(new Vector2(-0.2f, -1.4f));
                transform.eulerAngles = new Vector3(0, 0, -40f);
                break;
            case 2:
                transform.Translate(new Vector2(-0.8f, 0.6f));
                transform.eulerAngles = new Vector3(0, 0, 230f);
                break;
        }

        Step++;
    }
}
