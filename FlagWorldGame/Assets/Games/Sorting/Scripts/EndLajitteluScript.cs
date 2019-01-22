using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLajitteluScript : MonoBehaviour {

    private GameObject EndScore, Spawner;
    private Button ContinueB, NewGameB;
    private string LanguageID = "Language";
    private string CurrentGameHigh = "CurrGameHigh";
    private int Points;

    // Use this for initialization
    void Start() {
        EndScore = GameObject.Find("EndScore");
        Spawner = GameObject.Find("Spawner");
        //ContinueB = GameObject.Find("ContinueButton").GetComponent<Button>();
        //NewGameB = GameObject.Find("NewGame").GetComponent<Button>();
    }

    public void SetEnd()
    {
        Points = Spawner.GetComponent<LajitteluSpawner>().GetPoints();
        EndScore.GetComponent<Text>().text = "You scored " + Points.ToString() + " FP";
    }


    public void ClickContinue()
    {
        int OverallP = PlayerPrefs.GetInt("Points");
        OverallP += Points;
        PlayerPrefs.SetInt("Points", OverallP);
        if (PlayerPrefs.GetInt("Playmode") == 1)
        {
            SceneManager.LoadScene("TourMenu", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Lajittelu", LoadSceneMode.Single);
    }
}
