using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectHUD : MonoBehaviour
{
    private int Points = 0;
    private string CurrentGameHigh = "CurrGameHigh";
    private int P;
    private TMP_Text EndP, PointsT, TimerT, StartT, DetailsT;
    private Canvas End;
    private float timeLeft = 60f;
    private bool TimeEnd = true;
    private BoardManager BM;
    public TimerClockScript timerClockScript;

    private void Awake()
    {
        BM = GameObject.Find("BoardManager").GetComponent<BoardManager>();
    }

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        StartT = GameObject.Find("RulesText").GetComponent<TMP_Text>();
        DetailsT = GameObject.Find("Details").GetComponent<TMP_Text>();
        PointsT = GameObject.Find("Points").GetComponent<TMP_Text>();
        TimerT = GameObject.Find("Timer").GetComponent<TMP_Text>();
        EndP = GameObject.Find("EndPoints").GetComponent<TMP_Text>();
        End = GameObject.Find("End").GetComponent<Canvas>();
        SetText();
        if(!timerClockScript)
        {
            timerClockScript = FindObjectOfType<TimerClockScript>();
        }
        
        //if (PlayerPrefs.GetInt(CurrentGameHigh) != 0) // if restart
        //{
        //    StartCoroutine(WaitNewGame());
        //}
    }

    private void Update()
    {
        if (TimeEnd)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        timerClockScript.UpdateClock(timeLeft);

        if (Mathf.RoundToInt(timeLeft) <= 0)
        {
            timeLeft = 0;
            TimeEnd = true;
            TheEnd();
        }

        TimerT.text = "Time: " + Mathf.RoundToInt(timeLeft).ToString();
    }

    private void SetText()
    {
        PointsT.text = "Points: " + Points.ToString();
    }

    public void SetPoints(int Add)
    {
        Points += Add;
        SetText();
    }

    private void TheEnd()
    {
        if (Points >= 500)
        {
            P = 30;
        }
        else if (Points >= 400)
        {
            P = 25;
        }
        else if (Points >= 300)
        {
            P = 20;
        }
        else if (Points >= 200)
        {
            P = 15;
        }
        else if (Points >= 100)
        {
            P = 10;
        }
        else if (Points > 0)
        {
            P = 5;
        }
        else P = 0;

        EndP.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        EndP.text = "Points: " + Points.ToString();
        GameObject.Find("Continue").GetComponentInChildren<TMP_Text>().text = "Back to menu";  // End Button text set up

        if (P > PlayerPrefs.GetInt(CurrentGameHigh))
        {
            PlayerPrefs.SetInt(CurrentGameHigh, P);
        }
        else
        {
            P = PlayerPrefs.GetInt(CurrentGameHigh);
        }
        GetComponent<Canvas>().enabled = false;
        End.GetComponent<Canvas>().enabled = true;
    }

    public void ClickContinue()
    {
        // If game´will use some sort of highscore for combined gamescores
        //int CP = PlayerPrefs.GetInt("Points");
        //CP += P;
        //PlayerPrefs.SetInt("Points", CP);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void ClickStart()
    {
        GameObject[] Tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in Tiles)
        {
            tile.GetComponent<ConnectTile>().Started = true;
        }
        Camera.main.GetComponent<StartSwipe>().enabled = false;
        BM.UP = false;
        GameObject.Find("Start").GetComponent<Canvas>().enabled = false;
        GetComponent<Canvas>().enabled = true;
        timerClockScript.InitializeClock(timeLeft);
        TimeEnd = false;
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Connect", LoadSceneMode.Single);
    }

    private IEnumerator WaitNewGame()
    {
        yield return new WaitForSeconds(0.01f);
        ClickStart();
    }
}
