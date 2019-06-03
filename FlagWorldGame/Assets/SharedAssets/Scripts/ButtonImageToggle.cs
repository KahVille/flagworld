// Script for changing the toggle button image from off to on and back.

using UnityEngine;
using UnityEngine.UI;

public class ButtonImageToggle : MonoBehaviour
{
    // Image to be changed
    public Image changeImg;
    // Images for on and off state
    public Sprite offImg;
    public Sprite onImg;

    // This is used for mute button
    public void ToggleButtonImg()
    {
        if(changeImg.sprite == offImg)
        {
            changeImg.sprite = onImg;
        }
        else if(changeImg.sprite == onImg)
        {
            changeImg.sprite = offImg;
        }
    }

    // Toggle for gps, we dont change the sprite if gps is initializing so you can't spam the button.
    public void ToggleGPSButtonImg()
    {
        if(!FindObjectOfType<GPSScript>().IsInitializing)
        {
            if(changeImg.sprite == offImg)
            {
                changeImg.sprite = onImg;
                FindObjectOfType<GPSScript>().ToggleGPS();
            }
            else if(changeImg.sprite == onImg)
            {
                changeImg.sprite = offImg;
                FindObjectOfType<GPSScript>().ToggleGPS();
            }
        }
    }

    public void SetImg(bool isOn)
    {
        if(isOn)
        {
            changeImg.sprite = onImg;
        }
        else
        {
            changeImg.sprite = offImg;
        }
    }
}
