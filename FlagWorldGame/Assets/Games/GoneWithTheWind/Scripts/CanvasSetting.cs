using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetting : MonoBehaviour
{
    public GameObject textBG;
    public GameObject EndWBG;
    public GameObject EndLBG;

    // Start is called before the first frame update
    void Start()
    {
        if (Screen.height / Screen.width <= 1.5f)
        {
            textBG.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1150, 0);
            EndWBG.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1200, 0);
            EndLBG.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 1200, 0);
        }
    }

}
