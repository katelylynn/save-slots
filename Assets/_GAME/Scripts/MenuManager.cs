using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] RawImage slot_one_selected;
    [SerializeField] RawImage slot_two_selected;
    [SerializeField] RawImage slot_three_selected;

    [SerializeField] GameObject menu;
    bool isMenuEnabled;

    int index;

    void Start()
    {
        isMenuEnabled = false;
        menu.SetActive(false);
    }

    void Update()
    {
        if (isMenuEnabled)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                index--;
                ChangeSlot(index);

            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                index++;
                ChangeSlot(index);

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectSlot();
            }
        }
    }


    public void ShowMenu()
    {
        index = 0;
        menu.SetActive(true);
        isMenuEnabled = true;

        ChangeSlot(index);
    }

    void ChangeSlot(int i) {

        switch (i) {
            case -1:
                index++;
                break;
            case 0:
                slot_one_selected.enabled = true;
                slot_two_selected.enabled = false;
                slot_three_selected.enabled = false;
                break;
            case 1:
                slot_one_selected.enabled = false;
                slot_two_selected.enabled = true;
                slot_three_selected.enabled = false;
                break;
            case 2:
                slot_one_selected.enabled = false;
                slot_two_selected.enabled = false;
                slot_three_selected.enabled = true;
                break;
            case 3:
                index--;
                break;
            default:
                Debug.LogWarning("MENU INDEX ERROR");
                break;
        }
    }

    void SelectSlot()
    {
        isMenuEnabled = false;
        RecordingManager.SaveScript(index);
        EventManager.ResetLevel?.Invoke();
        menu.SetActive(false);
    }
}
