using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{

    static GameObject menu;

    // Menu is hidden when game starts
    void Start()
    {
        menu = GameObject.Find("SaveMenu");
        DisplayMenu(false);
    }

    // Display (true) or hide (false) the menu
    public void DisplayMenu(bool choice)
    {
        menu.SetActive(choice);
        Cursor.visible = choice;
    }

    // Resets the level with the slot the user selected
    public static void SelectSlot(int index)
    {
        RecordingManager.SaveScript(index);
        EventManager.ResetLevel?.Invoke();
        
        menu.SetActive(false);
        Cursor.visible = false;

    }
}
