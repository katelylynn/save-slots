using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingManager : MonoBehaviour
{
    public static int TicksPerSecond = 20;
    public static RecordingManager Instance;

    static GameObject[] p_Echoes = new GameObject[3];
    public static List<State> CurrentScript = new List<State>();
    public static string CurrentItem = "Empty";
    public static State[][] AllScripts = new State[][] { new State[0], new State[0], new State[0] };
    public static List<EchoController> Echoes = new List<EchoController>();
    public static bool[] WrittenScripts = new bool[] { false, false, false };

    float dur = 1f / TicksPerSecond;
    private float _t;
    bool Recording = false;


    // Start is called before the first frame update
    void Start()
    {
        Instance = GetComponent<RecordingManager>();
        p_Echoes[0] = Resources.Load<GameObject>("Prefabs/Echo0");
        p_Echoes[1] = Resources.Load<GameObject>("Prefabs/Echo1");
        p_Echoes[2] = Resources.Load<GameObject>("Prefabs/Echo2");
        EventManager.PlayerDeath += StopRecording;
        EventManager.ResetLevel += Begin;
        Begin();
    }

    // Summary:
    //     Manages timing for the recording system, so that it only logs a state TicksPerSecond
    //     times every second. 
    void Update()
    {
        if (Recording)
        {
            // Every TickPerSecond
            _t += Time.deltaTime;
            int max = 3;
            while (_t > dur && max > 0)
            {
                _t -= dur;
                max--;
                LogState();
            }
        }

    }
    // Summary:
    //     Called when the level is reset (by GameManager) to begin recording Player movements.
    public static void BeginRecording()
    {
        Instance.Recording = true;
    }

    // Summary:
    //     Called from the PlayerDeath event to stop recording Player movements.
    public static void StopRecording()
    {
        Instance.Recording = false;
    }

    // Summary:
    //     Called every script tick (by Update), requests the current state of the player and logs
    //     it in a master record.
    public static void LogState()
    {
        State s = PlayerController.GetState(CurrentItem, out bool u);
        if (u)
            CurrentItem = s.Item;
        CurrentScript.Add(s);
    }

    // Summary:
    //     Called by MenuManager to save the past Player life to one of the three save slots.
    public static void SaveScript(int index)
    {
        AllScripts[index] = CurrentScript.ToArray();

        WrittenScripts[index] = true;
        CurrentScript.Clear();
    }

    // Summary:
    //     Spawns an Echo for each of the Player's filled save slots.
    public static void SpawnEchoes()
    {
        for (int i = 0; i < 3; i++)
        {
            if (WrittenScripts[i])
            {
                SpawnEcho(i);
            }
        }
    }

    // Summary:
    //     Called when the level is reset to spawn an Echo for one of the Player's save slots.
    public static void SpawnEcho(int index)
    {
        EchoController sc = Instantiate(p_Echoes[index]).GetComponent<EchoController>();
        Echoes.Add(sc);
        sc.BeginLife(AllScripts[index]);
    }

    // Summary:
    //     Called when the level is reset. Clears past Echoes. 
    public static void Begin()
    {
        Echoes.Clear();
        BeginRecording();
        SpawnEchoes();
    }
}

public class State
{
    public Vector3 Position;
    public bool Direction; // Right is true
    public Pose Pose;
    public string Item;

    public State(Vector3 pos, bool dir, Pose p, string i)
    {
        Position = pos;
        Direction = dir;
        Pose = p;
        Item = i;
    }
}