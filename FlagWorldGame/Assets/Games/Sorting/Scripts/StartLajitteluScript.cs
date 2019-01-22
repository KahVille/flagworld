using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLajitteluScript : MonoBehaviour {

    private GameObject HUD, Spawner;
    private Text Rules, Details;
    private Button StartB;
    private string LanguageID = "Language";
    private string CurrentGameHigh = "CurrGameHigh";

    // Use this for initialization
    void Start () {
        HUD = GameObject.Find("HUD");
        Spawner = GameObject.Find("Spawner");
        Rules = GameObject.Find("RulesText").GetComponentInChildren<Text>();
        Details = GameObject.Find("Details").GetComponentInChildren<Text>();
        StartB = GetComponentInChildren<Button>();

        if(PlayerPrefs.GetInt(LanguageID) == 1)
        {
            Rules.text = "Sort fishes and trash properly by swiping the screen in right direction.\n\nSwipe the screen for more information.";
            Details.text = "Mainland fish go up, sea fishes go down, bio-waste goes to right and other trash goes to left.\n\nYou have 30 seconds.";
            StartB.GetComponentInChildren<Text>().text = "Start";
        }
        if(PlayerPrefs.GetInt(CurrentGameHigh) != 0)
        {
            ClickStart();
        }
	}
	
    public void ClickStart()
    {
        HUD.GetComponent<Canvas>().enabled = true;
        Spawner.GetComponent<LajitteluSpawner>().enabled = true;
        GetComponent<Canvas>().enabled = false;
        Camera.main.GetComponent<StartSwipe>().enabled = false;
        Camera.main.GetComponent<InputLajittelu>().enabled = true;
    }
}
