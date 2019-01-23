using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    private Canvas Games;

    private void Start()
    {
        Games = GameObject.Find("GamesCanvas").GetComponent<Canvas>();
    }

    public void ClickGames()
    {
        GetComponent<Canvas>().enabled = false;
        Games.enabled = true;
    }
}
