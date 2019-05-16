using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LajitteluSpawner : MonoBehaviour
{

    public GameObject Thing;
    public float playTime = 30f;

    private bool Spawn = true;
    private bool TimeEnd = false;

    private GameObject HUD, End;
    private Text Top, Bottom, Right, Left;
    private TMP_Text Points, Timer;

    private string LanguageID = "Language";
    private string timeText = "Aika: ";

    private int P = 0, WP = 0;
    private InputLajittelu IL;
    // Used to update the timer clock
    public TimerClockScript timerClockScript;

    // Use this for initialization
    void Start()
    {
        HUD = GameObject.Find("HUD");
        End = GameObject.Find("EndCanvas");
        Points = GameObject.Find("PointsText").GetComponent<TMP_Text>();
        Timer = GameObject.Find("TimerText").GetComponent<TMP_Text>();
        IL = GetComponent<InputLajittelu>();
        if(!timerClockScript)
        {
            timerClockScript = FindObjectOfType<TimerClockScript>();
        }
        timerClockScript.InitializeClock(playTime);
        SetText();
    }

    private void SetText()
    {
        timeText = "Time: ";

        SetPoints();
    }

    private void SetPoints()
    {
        Points.text = "Points: " + P.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimeEnd)
        {
            playTime -= Time.deltaTime;
            timerClockScript.UpdateClock(playTime);
            Timer.text = timeText + Mathf.RoundToInt(playTime);
            if(playTime <= 0)
            {
                TimeEnd = true;
                Spawn = false;
                StartCoroutine(Ending());
            }
        }

        if (!Spawn)
        {
            return;
        }
        Spawn = false;

        IL.SetSortable(Instantiate(Thing, new Vector3(0, 0, -1), Quaternion.identity));

    }

    public void RightDir()
    {
        Points.color = new Color32(4, 191, 4, 255);
        P++;
        SetPoints();
        StartCoroutine(ShowFeed());
    }

    public void WrongDir()
    {
        Points.color = new Color32(219, 5, 0, 255);
        WP++;
        if (WP == 5)
        {
            P--;
            WP = 0;
        }
        if (P < 0)
        {
            P = 0;
        }
        SetPoints();
        StartCoroutine(ShowFeed());
    }

    private IEnumerator ShowFeed()
    {
        yield return new WaitForSeconds(0.5f);
        Points.color = new Color32(0, 0, 0, 255);
    }

    private IEnumerator Ending()
    {
        yield return new WaitForSeconds(1);
        End.GetComponent<EndLajitteluScript>().SetEnd();
        End.GetComponent<Canvas>().enabled = true;
        HUD.GetComponent<Canvas>().enabled = false;
    }

    public int GetPoints()
    {
        return P;
    }

    public void SetSpawn()
    {
        Spawn = !Spawn;
    }

}
