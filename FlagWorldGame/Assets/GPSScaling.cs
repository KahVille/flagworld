using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSScaling : MonoBehaviour
{
    public Image HelpI, CreditsI;

    private void Awake()
    {
        if (Screen.height / Screen.width <= 1.5f)
        {
            HelpI.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 1150);
            CreditsI.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 1150);
        }
    }
}
