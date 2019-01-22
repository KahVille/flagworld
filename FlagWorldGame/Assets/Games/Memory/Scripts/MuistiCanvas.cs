using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MuistiCanvas : MonoBehaviour {

    private Canvas StartC, HUD, End;
    private string LanguageID = "Language";
    private string CurrentGameHigh = "CurrGameHigh";
    private int LID, points;

	// Use this for initialization
	void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
        StartC = GameObject.Find("StartCanvas").GetComponent<Canvas>();
        HUD = GameObject.Find("HUD").GetComponent<Canvas>();
        End = GameObject.Find("EndCanvas").GetComponent<Canvas>();
    }

    public void TheEnd(int moves, bool finished, int pairs)
    {
        if (finished)
        {
            points = 30;
            if(moves > 20)
            {
                points = 30 - (moves - 20);
                if (points < 0)
                {
                    points = 0;
                }
            }

            GameObject.Find("Points").GetComponent<TMP_Text>().text = "You made " + moves.ToString() + " moves\nYou scored " + points.ToString() + " MP";

        }
        else
        {
            points = pairs * 3 - 5;
            if (moves > 20)
            {
                points = points - (moves - 20);
            }
            if (points < 0)
            {
                points = 0;
            }

            GameObject.Find("Points").GetComponent<TMP_Text>().text = "You opened " + pairs.ToString() + " pairs in " + moves.ToString() + " moves\nYou scored " + points.ToString() + " MP";
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
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ClickNewGame()
    {
        SceneManager.LoadScene("Muisti", LoadSceneMode.Single);
    }

    private IEnumerator NewGameStart()
    {
        yield return new WaitForSeconds(0.1f);
        ClickStart();
    }
}
