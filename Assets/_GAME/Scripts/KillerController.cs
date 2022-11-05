using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerController : MonoBehaviour
{
    static Animator anim;
    static GameObject p_Laser;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        p_Laser = Resources.Load<GameObject>("Prefabs/Laser");
        EventManager.ResetLevel += OnReset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Attack()
    {
        anim.SetTrigger("Death 0");
        LeanTween.delayedCall(2.1f, e => Instantiate(p_Laser));
    }

    void OnReset()
    {
        anim.Play("Door", 0, 0);
    }
}
