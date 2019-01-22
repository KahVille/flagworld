using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LajitteluBackground : MonoBehaviour {

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;

        transform.localScale = new Vector3(width, height, transform.localScale.z);
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
