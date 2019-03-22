using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectHUD : MonoBehaviour
{
    private int Points = 0;
    private string LanguageID = "Language";
    private string CurrentGameHigh = "CurrGameHigh";
    private string ModeID = "Playmode";
    private int LID, P;
    private Text PointsT, TimerT, EndP, StartT, DetailsT;
    private Canvas End;
    private float timeLeft = 30f;
    private bool TimeEnd = true;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        LID = PlayerPrefs.GetInt(LanguageID);
        StartT = GameObject.Find("RulesText").GetComponent<Text>();
        DetailsT = GameObject.Find("Details").GetComponent<Text>();
        if (LID != 1)
        {
            GameObject.Find("StartB").GetComponentInChildren<Text>().text = "Aloita";
            StartT.text = "Vaihda kuvan paikkaa viereisen kuvan kanssa ja yritä saada aikaiseksi kolmen suoria.\n\n Pyyhkäise ruutua saadaksesi lisäohjeita.";
            DetailsT.text = "Valmiit suorat katoavat ruudulta.\n\n Sinulla on 30 sekuntia aikaa. ";
        }
        PointsT = GameObject.Find("Points").GetComponent<Text>();
        TimerT = GameObject.Find("Timer").GetComponent<Text>();
        EndP = GameObject.Find("EndPoints").GetComponent<Text>();
        End = GameObject.Find("End").GetComponent<Canvas>();
        SetText();
        if (PlayerPrefs.GetInt(CurrentGameHigh) != 0)
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

        if (Mathf.RoundToInt(timeLeft) <= 0)
        {
            timeLeft = 0;
            TimeEnd = true;
            TheEnd();
        }
        if (LID == 1)
        {
            TimerT.text = "Time: " + Mathf.RoundToInt(timeLeft).ToString();
        }
        else
        {
            TimerT.text = "Aika: " + Mathf.RoundToInt(timeLeft).ToString();
        }
    }

    private void SetText()
    {
        if (LID == 1)
        {
            PointsT.text = "Points: " + Points.ToString();
        }
        else
        {
            PointsT.text = "Pisteet: " + Points.ToString();
        }
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
        if (LID == 1)
        {
            EndP.text = "Points: " + Points.ToString() + "\nIt equals to " + P.ToString() + " MP";
            if (PlayerPrefs.GetInt("Playmode") == 1)
            {
                GameObject.Find("Continue").GetComponentInChildren<Text>().text = "Continue tour";
            }
            else
            {
                GameObject.Find("Continue").GetComponentInChildren<Text>().text = "Back to menu";
            }
        }
        else
        {
            EndP.text = "Pisteet: " + Points.ToString() + "\nSe tarkoittaa " + P.ToString() + " MP";
            if (PlayerPrefs.GetInt("Playmode") == 1)
            {
                GameObject.Find("Continue").GetComponentInChildren<Text>().text = "Jatka kierrosta";
            }
            else
            {
                GameObject.Find("Continue").GetComponentInChildren<Text>().text = "Palaa valikkoon";
            }
            GameObject.Find("NewGame").GetComponentInChildren<Text>().text = "Uusi yritys";
        }
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
        int CP = PlayerPrefs.GetInt("Points");
        CP += P;
        PlayerPrefs.SetInt("Points", CP);
        if (PlayerPrefs.GetInt(ModeID) == 1)
        {
            SceneManager.LoadScene("TourMenu", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    public void ClickStart()
    {
        GameObject[] Tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in Tiles)
        {
            tile.GetComponent<ConnectTile>().Started = true;
        }
        Camera.main.GetComponent<StartSwipe>().enabled = false;
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
