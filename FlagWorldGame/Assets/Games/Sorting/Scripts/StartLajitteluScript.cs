using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartLajitteluScript : MonoBehaviour {

    private GameObject HUD, Spawner;
    private Button StartB;
    private string LanguageID = "Language";
    private string CurrentGameHigh = "CurrGameHigh";
    private RawImage TopBanner;
    private TMP_Text Europe;

    // Use this for initialization
    void Start () {
        HUD = GameObject.Find("HUD");
        Spawner = GameObject.Find("Spawner");
        StartB = GetComponentInChildren<Button>();
        Europe = GameObject.Find("Top").GetComponent<TMP_Text>();
        TopBanner = GameObject.Find("TopBanner").GetComponent<RawImage>();

        TopBanner.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 6);
        Europe.rectTransform.anchoredPosition = new Vector3(0, -(Screen.height / 12 + 50));

    }
	
    public void ClickHard()
    {
        Spawner.GetComponent<LajitteluSpawner>().SetHard();
        ClickStart();
    }

    public void ClickStart()
    {
        HUD.GetComponent<Canvas>().enabled = true;
        Spawner.GetComponent<LajitteluSpawner>().enabled = true;
        GetComponent<Canvas>().enabled = false;
        Spawner.GetComponent<InputLajittelu>().enabled = true;
        Spawner.GetComponent<LajitteluSpawner>().enabled = true;
    }
}
