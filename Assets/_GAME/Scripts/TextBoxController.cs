using UnityEngine;
using TMPro;
using System.Collections;

public class TextBoxController : MonoBehaviour
{
    [SerializeField] GameObject text_box;
    [SerializeField] TMP_Text text;

    bool isComplete;
    int index = 0;
    string[] currentMonologue;

    string[] endMessage = new string[] {
        "How did it come to this? How did you find yourself so completely alone?", 
        "Do you still think about it? Do you even remember?", 
        "The moment that changed your life.",
        "I think you do remember. I think you curse yourself every night for choosing to help that man.", 
        "Homeless, disheveled, lacking food or even proper clothes. When hundreds of people passed by a starving man without another thought, you invited him in.", 
        "And then he traded lives with you.",
        "Ten years of barely scraping by, losing everything you love in your life.", 
        "So incredibly alone.", 
        "And then, out of the blue, someone comes knocking to take the very last thing you own."
    };

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isComplete)
        {
            Debug.Log(index + "/" + (currentMonologue.Length - 1));

            if (index > currentMonologue.Length - 1)
                EndMonologue();
            else
                PrintMessage();
        }
    }

    public void StartMonologue(string[] lines)
    {
        currentMonologue = lines;
        index = 0;
        PrintMessage();
        text_box.SetActive(true);
    }

    public void StartMonologue(string[] lines, float delay)
    {
        LeanTween.delayedCall(delay, e => StartMonologue(lines));
    }

    public void EndMonologue()
    {
        text_box.SetActive(false);
        if (DeathController.IsPlayerDead)
            UIController.ShowMenu();
    }

    void PrintMessage()
    {
        isComplete = false;
        text.maxVisibleCharacters = 0;
        text.text = currentMonologue[index];
        LeanTween.value(gameObject, 0f, text.text.Length, text.text.Length * 0.01f)
                 .setOnUpdate(e => text.maxVisibleCharacters = (int)e)
                 .setOnComplete(() => { isComplete = true; });
        index++;
    }
}
