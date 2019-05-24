using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndLajitteluScript : MonoBehaviour {

    private GameObject EndScore, Spawner;
    private Button ContinueB, NewGameB;
    private string SortingHigh = "SortingHigh";
    private int Points;

    // Use this for initialization
    void Start() {
        EndScore = GameObject.Find("EndScore");
        Spawner = GameObject.Find("Spawner");
    }

    public void SetEnd()
    {
        Points = Spawner.GetComponent<LajitteluSpawner>().GetPoints();
                string localized_youscored = (LocalizationManager.Instance != null) ?
                                $"{LocalizationManager.Instance.GetLocalizedValue("points_earned")} {Points.ToString()} P \n"
                                : $"You scored {Points.ToString()} points \n";
        EndScore.GetComponent<TMP_Text>().text = localized_youscored;
        if(Points > PlayerPrefs.GetInt(SortingHigh))
        {
            string localized_newHigh = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("new_highscore_text")
                                    : "New highscore";
            EndScore.GetComponent<TMP_Text>().text += localized_newHigh;
            PlayerPrefs.SetInt(SortingHigh, Points);
        }
        else
        {
            string localized_highscore = (LocalizationManager.Instance != null) ? $"{LocalizationManager.Instance.GetLocalizedValue("highscore_text")} {PlayerPrefs.GetInt(SortingHigh).ToString()} P \n"
                                    : $"Highscore {PlayerPrefs.GetInt(SortingHigh).ToString()}";
            EndScore.GetComponent<TMP_Text>().text += localized_highscore;
        }
    }


    public void ClickContinue()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Sorting", LoadSceneMode.Single);
    }
}
