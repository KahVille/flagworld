using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LajitteluBackground : MonoBehaviour {

    public Material FinPic, EngPic;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;

        if(PlayerPrefs.GetInt("Language") == 0)
        {
            GetComponent<MeshRenderer>().material = FinPic;
        }
        else
        {
            GetComponent<MeshRenderer>().material = EngPic;
        }

        transform.localScale = new Vector3(width, height, transform.localScale.z);
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
