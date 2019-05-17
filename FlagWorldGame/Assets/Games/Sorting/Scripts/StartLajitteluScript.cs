using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartLajitteluScript : MonoBehaviour {

    private GameObject HUD, Spawner;
    private Button StartB;

    // Use this for initialization
    void Start () {
        HUD = GameObject.Find("HUD");
        Spawner = GameObject.Find("Spawner");
        StartB = GetComponentInChildren<Button>();

    }
	
    public void ClickHard()
    {
        Spawner.GetComponent<LajitteluSpawner>().SetHard();
        ClickStart();
    }

    public void ClickStart()
    {
        HUD.GetComponent<Canvas>().enabled = true;
        Spawner.GetComponent<LajitteluSpawner>().enabled = true;
        GetComponent<Canvas>().enabled = false;
        Spawner.GetComponent<InputLajittelu>().enabled = true;
        Spawner.GetComponent<LajitteluSpawner>().enabled = true;
    }
}
