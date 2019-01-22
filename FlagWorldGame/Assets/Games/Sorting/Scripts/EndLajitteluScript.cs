using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    }

    public void SetEnd()
    {
        Points = Spawner.GetComponent<LajitteluSpawner>().GetPoints();
        EndScore.GetComponent<TMP_Text>().text = "You scored " + Points.ToString() + " FP";
    }


    public void ClickContinue()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Lajittelu", LoadSceneMode.Single);
    }
}
