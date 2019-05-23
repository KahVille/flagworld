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
    private string ConnectHigh = "ConnectHigh";
    private int P;
    private TMP_Text EndP, PointsT, TimerT, StartT, DetailsT;
    private Canvas End;
    private float timeLeft = 60f;
    private bool TimeEnd = true;
    private BoardManager BM;
    public TimerClockScript timerClockScript;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if(Screen.height / Screen.width >= 2)
        {
            Camera.main.orthographicSize = 6.3f;
        }
        else
        {
            Camera.main.orthographicSize = 5.5f;
        }
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

        if (PlayerPrefs.GetInt(CurrentGameHigh) == 1) // if restart
        {
            StartCoroutine(WaitNewGame());
        }
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

        TimerT.text = Mathf.RoundToInt(timeLeft).ToString();
    }

    private void SetText()
    {
        PointsT.text = "P: " + Points.ToString();
    }

    public void SetPoints(int Add)
    {
        Points += Add;
        SetText();
    }

    private void TheEnd()
    {
        EndP.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        EndP.text = LocalizationManager.Instance.GetLocalizedValue("points_text").ToUpper() + " " + Points.ToString();
        if(Points > PlayerPrefs.GetInt(ConnectHigh))
        {
            EndP.text += "\n" + LocalizationManager.Instance.GetLocalizedValue("new_highscore_text");
            PlayerPrefs.SetInt(ConnectHigh, Points);
        }
        else
        {
            EndP.text += "\n" + LocalizationManager.Instance.GetLocalizedValue("highscore_text") + " "+ PlayerPrefs.GetInt(ConnectHigh).ToString();
        }

        GetComponent<Canvas>().enabled = false;
        End.GetComponent<Canvas>().enabled = true;
    }

    public void ClickContinue()
    {
        PlayerPrefs.SetInt(CurrentGameHigh, 0);
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
        PlayerPrefs.SetInt(CurrentGameHigh, 1);
        SceneManager.LoadScene("Connect", LoadSceneMode.Single);
    }

    private IEnumerator WaitNewGame()
    {
        yield return new WaitForSeconds(0.01f);
        ClickStart();
    }
}
