using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    private Canvas Games;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Games = GameObject.Find("GamesCanvas").GetComponent<Canvas>();
        PlayerPrefs.SetInt("CurrGameHigh", 0);
    }

    public void ClickGames()
    {
        GetComponent<Canvas>().enabled = false;
        Games.enabled = true;
    }
}
