using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectHUD : MonoBehaviour
{
    private int Points = 0;
    private string CurrentGameHigh = "CurrGameHigh";
    private int P;
    private Text PointsT, TimerT, EndP, StartT, DetailsT;
    private Canvas End;
    private float timeLeft = 30f;
    private bool TimeEnd = true;
    private BoardManager BM;

    private void Awake()
    {
        BM = GameObject.Find("BoardManager").GetComponent<BoardManager>();
    }

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        StartT = GameObject.Find("RulesText").GetComponent<Text>();
        DetailsT = GameObject.Find("Details").GetComponent<Text>();
        PointsT = GameObject.Find("Points").GetComponent<Text>();
        TimerT = GameObject.Find("Timer").GetComponent<Text>();
        EndP = GameObject.Find("EndPoints").GetComponent<Text>();
        End = GameObject.Find("End").GetComponent<Canvas>();
        SetText();
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
        EndP.text = "Points: " + Points.ToString() + "\nIt equals to " + P.ToString() + " MP";
        GameObject.Find("Continue").GetComponentInChildren<Text>().text = "Back to menu";

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
