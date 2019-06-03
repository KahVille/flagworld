// Script which shows up and down arrow on description text when it's possible to scroll.
using System;
using System.Collections;
using UnityEngine;

public class LocationDescription : MonoBehaviour
{
    RectTransform rt;
    public GameObject upArrow;
    public GameObject downArrow;
    bool canScroll;
    float initialOffsetMin;

    private void Awake() 
    {
        rt = GetComponent<RectTransform>(); 
    }

    private void OnDisable() 
    {
        upArrow.SetActive(false);
        downArrow.SetActive(false);    
    }

    public void NeedCustomUpdate()
    {
        Debug.Log(rt.offsetMin.y);
        if(rt.offsetMin.y >= -500.0f)
        {
            upArrow.SetActive(false);
            downArrow.SetActive(false);
            canScroll = false;
        }
        else
        {
            canScroll = true;
            initialOffsetMin = rt.offsetMin.y;
            StartCoroutine(CustomUpdate());
        }
    }

    // Update as ienumerator, will be called only when needed
    IEnumerator CustomUpdate()
    {
        while(true)
        {
            if(rt.anchoredPosition.y <= 10.0f && canScroll)
            {
                upArrow.SetActive(false);
                downArrow.SetActive(true);
            }
            else if(rt.anchoredPosition.y >= (-initialOffsetMin * 0.5f) + 10.0f && canScroll)
            {
                downArrow.SetActive(false);
                upArrow.SetActive(true);
            }
            else if(rt.anchoredPosition.y < (-initialOffsetMin * 0.5f) + 10.0f && rt.anchoredPosition.y > 10.0f)
            {
                downArrow.SetActive(true);
                upArrow.SetActive(true);
            }

            // if(rt.anchoredPosition.y < (-initialOffsetMin * 0.5f) + 10.0f && rt.anchoredPosition.y > 10.0f)
            // {
            //     upArrow.SetActive(true);
            //     downArrow.SetActive(true);
            // }

            // if(rt.anchoredPosition.y > 10.0f && canScroll && upArrow.activeSelf)
            // {
            //     upArrow.SetActive(false);
            // }
            // if(rt.anchoredPosition.y < -rt.offsetMin.y + 10.0f && canScroll)
            // {
            //     downArrow.SetActive(false);
            // }
            yield return null;
        }
    }

    // private GUIStyle guiStyle = new GUIStyle();

    // private void OnGUI() 
    // {
    //     guiStyle.fontSize = 100;
    //     GUI.Label(new Rect(10, 10, 400, 50), rt.anchoredPosition.y.ToString(), guiStyle);
    //     GUI.Label(new Rect(600, 10, 400, 50), ((-initialOffsetMin * 0.5f) + 10.0f).ToString(), guiStyle);
    // }
}
