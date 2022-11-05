using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        EventManager.ResetLevel += Begin;
        Begin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            KillerController.Attack();
    }

    public static void Begin()
    {
        RecordingManager.BeginRecording();
        SpawnEchos();
    }

    public static void SpawnEchos()
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
