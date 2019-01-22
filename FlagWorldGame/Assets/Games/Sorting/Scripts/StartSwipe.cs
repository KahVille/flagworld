using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSwipe : MonoBehaviour
{

    private bool tap, swipe;
    public bool FirstScreen;
    private bool isDragging;
    private Vector2 startTouch, swipeDelta;
    Text details, rulestext;
    private Swipe SW;


    void Start()
    {
        SW = GetComponent<Swipe>();
        details = GameObject.Find("Details").GetComponent<Text>();
        rulestext = GameObject.Find("RulesText").GetComponent<Text>();
        details.enabled = true;
        FirstScreen = true;
        if (FirstScreen)
        {
            details.enabled = false;
        }
    }

    private void Update()
    {
        if (SW.SwipeLeft || SW.SwipeRight || SW.SwipeUp || SW.SwipeDown)
        {
            change();
        }
    }

    private void change()
    {
        if (FirstScreen)
        {
            FirstScreen = false;
            rulestext.enabled = true;
            details.enabled = false;
        }
        else
        {
            FirstScreen = true;
            rulestext.enabled = false;
            details.enabled = true;
        }
    }
    

}
