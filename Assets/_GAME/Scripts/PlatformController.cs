using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] float heightThreshold;
    float h;

    BoxCollider2D bCollider;

    // Start is called before the first frame update
    void Start()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        h = transform.position.y + heightThreshold;

        if (PlayerController.MyPosition.y >= h != bCollider.enabled)
            bCollider.enabled = PlayerController.MyPosition.y >= h;
    }

    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position + new Vector3(0.5f, heightThreshold - 0.975f, 0), transform.position + new Vector3(-0.5f, heightThreshold - 0.975f, 0), Color.red);
    }
}
