using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MuistiCanvas : MonoBehaviour {

    private Canvas StartC, HUD, End;
    private string CurrentGameHigh = "CurrGameHigh";
    private int points;

	// Use this for initialization
	void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
        StartC = GameObject.Find("StartCanvas").GetComponent<Canvas>();
        HUD = GameObject.Find("HUD").GetComponent<Canvas>();
        End = GameObject.Find("EndCanvas").GetComponent<Canvas>();
        if(PlayerPrefs.GetInt(CurrentGameHigh) != 0)
        {
            StartCoroutine(NewGameStart());
        }
    }

    public void TheEnd(int moves, bool finished, int pairs)
    {
        if (finished)
        {
            GameObject.Find("Points").GetComponent<TMP_Text>().text = "You made " + moves.ToString() + " moves";
            if(moves > PlayerPrefs.GetInt(CurrentGameHigh))
            {
                PlayerPrefs.SetInt(CurrentGameHigh, moves);
            }
            else
            {
                moves = PlayerPrefs.GetInt(CurrentGameHigh);
            }
        }
        else
        {
            GameObject.Find("Points").GetComponent<TMP_Text>().text = "You opened " + pairs.ToString() + " pairs in " + moves.ToString() + " moves";
        }
        HUD.enabled = false;
        End.enabled = true;
    }

    public void ClickStart()
    {
        GameObject.FindGameObjectWithTag("Tile").GetComponent<MuistiTile>().StartPressed();
        StartC.enabled = false;
        HUD.enabled = true;
    }

    public void ClickContinue()
    {
        // Set Highscore using CurrGameHigh
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ClickNewGame()
    {
        SceneManager.LoadScene("Memory", LoadSceneMode.Single);
    }

    private IEnumerator NewGameStart()
    {
        yield return new WaitForSeconds(0.1f);
        ClickStart();
    }
}
