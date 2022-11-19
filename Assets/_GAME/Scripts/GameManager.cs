using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        EventManager.ResetLevel += Begin;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillerController.Attack();
    }

    // Summary:
    //     Called when the level is reset, to begin the recording process.
    public static void Begin()
    {
        
    }
}
