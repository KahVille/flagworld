// Script for changing the toggle button image from off to on and back.

using UnityEngine;
using UnityEngine.UI;

public class ButtonImageToggle : MonoBehaviour
{
    // Images for on and off state
    public Sprite offImg;
    public Sprite onImg;

    public void ToggleButtonImg()
    {
        if(GetComponent<Image>().sprite == offImg)
        {
            GetComponent<Image>().sprite = onImg;
        }
        else if(GetComponent<Image>().sprite == onImg)
        {
            GetComponent<Image>().sprite = offImg;
        }
    }
}
