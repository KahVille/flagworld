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
        EndScore.GetComponent<TMP_Text>().text = "You scored " + Points.ToString() + " points";
        if(Points > PlayerPrefs.GetInt(SortingHigh))
        {
            EndScore.GetComponent<TMP_Text>().text += "\nNew Highscore!";
            PlayerPrefs.SetInt(SortingHigh, Points);
        }
        else
        {
            EndScore.GetComponent<TMP_Text>().text += "\nHighscore: " + PlayerPrefs.GetInt(SortingHigh).ToString();
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
