using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSwipe : MonoBehaviour
{
    [HideInInspector] public bool SwipeOn = false;
    private ConnectTile TileScript;
    private string Direction;
    private Swipe SW;

    // Use this for initialization
    void Start()
    {
        TileScript = GetComponent<ConnectTile>();
        SW = Camera.main.GetComponent<Swipe>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SW.SwipeLeft)
        {
            Direction = "Left";
        }
        else if (SW.SwipeRight)
        {
            Direction = "Right";
        }
        else if (SW.SwipeUp)
        {
            Direction = "Up";
        }
        else if (SW.SwipeDown)
        {
            Direction = "Down";
        }
        else
        {
            Direction = "";
        }

        if (SwipeOn && Direction != "")
        {
            TileScript.SwipeData(Direction);
        }
    }
}
