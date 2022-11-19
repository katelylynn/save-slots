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
        Begin();
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
        RecordingManager.BeginRecording();
        SpawnEchoes();
    }

    // Summary:
    //     Spawns an Echo for each of the Player's filled save slots.
    public static void SpawnEchoes()
    {
        for (int i = 0; i < 3; i++)
        {
            if (RecordingManager.WrittenScripts[i])
            {
                RecordingManager.SpawnEcho(i);
            }
        }
    }
}
