using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public int DeathCount = 0;
    public static bool IsPlayerDead = false;
    public static string DeathEvent = "";

    // Start is called before the first frame update
    void Start()
    {
        EventManager.PlayerDeath += OnDeath;
        EventManager.ResetLevel += OnReset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDeath()
    {
        IsPlayerDead = true;
        DeathEvent = "death_" + DeathCount;

        DeathCount++;
    }

    void OnReset()
    {
        IsPlayerDead = false;
    }
}
