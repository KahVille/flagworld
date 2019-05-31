// Script which shows up and down arrow on description text when it's possible to scroll.
using System;
using System.Collections;
using UnityEngine;

public class LocationDescription : MonoBehaviour
{
    RectTransform rt;
    public GameObject upArrow;
    public GameObject downArrow;

    private void Awake() 
    {
        rt = GetComponent<RectTransform>(); 
    }

    private void OnEnable() 
    {
        Debug.Log(rt.offsetMin.y);
        if(rt.offsetMin.y >= -500.0f)
        {
            upArrow.SetActive(false);
            downArrow.SetActive(false);
        }
        else
        {
            StartCoroutine(CustomUpdate());
        }
    }

    // Update as ienumerator, will be called only when needed
    IEnumerator CustomUpdate()
    {
        while(true)
        {
            Debug.Log(rt.position.y);
            if(rt.position.y <= -500.0f && !upArrow.activeSelf)
            {
                upArrow.SetActive(true);
            }
            if(rt.offsetMin.y >= ((rt.offsetMin.y/2.0f) + 100.0f) && !downArrow.activeSelf)
            {
                downArrow.SetActive(true);
            }
            yield return null;
        }
    }
}
