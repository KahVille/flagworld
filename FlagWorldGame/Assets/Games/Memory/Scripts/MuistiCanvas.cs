using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MuistiCanvas : MonoBehaviour
{

    private Canvas StartC, HUD, End;
    private string CurrentGameHigh = "CurrGameHigh";
    private string MemoryHigh = "MemoryHigh";
    private int points;

    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        StartC = GameObject.Find("StartCanvas").GetComponent<Canvas>();
        HUD = GameObject.Find("HUD").GetComponent<Canvas>();
        End = GameObject.Find("EndCanvas").GetComponent<Canvas>();
        if (PlayerPrefs.GetInt(CurrentGameHigh) == 1)
        {
            StartCoroutine(NewGameStart());
        }
    }

    public void TheEnd(int moves, bool finished, int pairs)
    {
        if (finished)
        {
            string localized_youmade = (LocalizationManager.Instance != null) ?
                                $"{LocalizationManager.Instance.GetLocalizedValue("you_made")} {moves.ToString()} {LocalizationManager.Instance.GetLocalizedValue("moves_text_win")}  \n"
                                : $"You made {moves.ToString()} moves  \n";

            GameObject.Find("Points").GetComponent<TMP_Text>().text = localized_youmade;
            if (moves < PlayerPrefs.GetInt(MemoryHigh) || PlayerPrefs.GetInt(MemoryHigh) == 0)
            {
                string localized_newHigh = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("new_highscore_text")
                                    : "New highscore";
                GameObject.Find("Points").GetComponent<TMP_Text>().text += localized_newHigh;
                PlayerPrefs.SetInt(MemoryHigh, moves);
            }
            else
            {
                string localized_highscore = (LocalizationManager.Instance != null) ? $"{LocalizationManager.Instance.GetLocalizedValue("highscore_text")} {PlayerPrefs.GetInt(MemoryHigh).ToString()} \n"
                                    : $"Highscore {PlayerPrefs.GetInt(MemoryHigh).ToString()}";
                GameObject.Find("Points").GetComponent<TMP_Text>().text += localized_highscore;
            }
        }
        else
        {
            //is this condition true ? yes : no
            string localized_early_end = (LocalizationManager.Instance != null) ? 
            $"{LocalizationManager.Instance.GetLocalizedValue("you_opened")} {pairs.ToString()} {LocalizationManager.Instance.GetLocalizedValue("pairs_in")} \n {moves.ToString()} {LocalizationManager.Instance.GetLocalizedValue("moves_text")}   \n"
                                : $"You opened {pairs.ToString()} pairs in {moves.ToString()} moves   \n";
            GameObject.Find("Points").GetComponent<TMP_Text>().text = localized_early_end;
            if (PlayerPrefs.GetInt(MemoryHigh) != 0)
            {
                string localized_highscore = (LocalizationManager.Instance != null) ? $"{LocalizationManager.Instance.GetLocalizedValue("highscore_text")} {PlayerPrefs.GetInt(MemoryHigh).ToString()} \n"
                        : $"Highscore {PlayerPrefs.GetInt(MemoryHigh).ToString()}";
                GameObject.Find("Points").GetComponent<TMP_Text>().text += localized_highscore;
            }
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
        PlayerPrefs.SetInt(CurrentGameHigh, 0);
        SceneManager.LoadScene("GPSTest", LoadSceneMode.Single);
    }

    public void ClickNewGame()
    {
        PlayerPrefs.SetInt(CurrentGameHigh, 1);
        SceneManager.LoadScene("Memory", LoadSceneMode.Single);
    }

    private IEnumerator NewGameStart()
    {
        yield return new WaitForSeconds(0.1f);
        ClickStart();
    }
}
