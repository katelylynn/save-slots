using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class UIController : MonoBehaviour
{
    public static List<string> PrintEvents = new List<string>() { "death_0" };
    static UIController Instance;

    [SerializeField] RawImage fade;
    TextBoxController textBox;
    MenuManager menu;

    // Start is called before the first frame update
    void Start()
    {
        menu = GetComponent<MenuManager>();
        textBox = GetComponent<TextBoxController>();
        Instance = GetComponent<UIController>();
        EventManager.PlayerDeath += OnDeath;
        EventManager.ResetLevel += OnReset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDeath()
    {
        fade.gameObject.SetActive(true);
        LeanTween.value(0f, 0.85f, 4f)
                 .setEaseInOutQuad()
                 .setOnUpdate(a => fade.color = new Color(0f, 0f, 0f, a))
                 .setOnComplete(OnUIShow);
    }

    public void OnUIShow()
    {
        EventManager.StartUI?.Invoke();

        if (PrintEvents.Contains(DeathController.DeathEvent))
            MonologueEvent(DeathController.DeathEvent);
        else
            menu.ShowMenu();
    }

    public void OnReset()
    {
        fade.color = Color.clear;
        fade.gameObject.SetActive(false);
    }
    
    public static void MonologueEvent(string id)
    {
        TextAsset file = Resources.Load<TextAsset>("Monologues/" + id);
        Monologue m = JsonConvert.DeserializeObject<Monologue>(file.text);

        if (m.delay > 0)
            Instance.textBox.StartMonologue(m.lines, m.delay);
        else
            Instance.textBox.StartMonologue(m.lines);
    }

    public static void ShowMenu()
    {
        Instance.menu.ShowMenu();
    }
}

public class Monologue
{
    public float delay;
    public string audioEvent;
    public string[] lines;

    public Monologue()
    {

    }
}