// Script responsible for updating radial clock

using UnityEngine;
using UnityEngine.UI;

public class TimerClockScript : MonoBehaviour
{
    // Put the radial image in this variable
    public Image radialImg;
    // Variable used to check if the clock should be updating
    bool isOn;
    float maxTime;
    float currentTime;

    private void Start() 
    {
        if(!radialImg)
        {
            radialImg = transform.GetChild(1).GetComponent<Image>();
        }
    }

    private void Update() 
    {
        if(isOn)
        {
            radialImg.fillAmount = currentTime / maxTime;
        }
    }

    public void InitializeClock(float newMaxTime)
    {
        maxTime = newMaxTime;
        currentTime = maxTime;
        radialImg.fillAmount = currentTime / maxTime;
        isOn = true;
    }

    public void StopClock()
    {
        isOn = false;
    }

    public void UpdateClock(float newCurrentTime)
    {
        currentTime = newCurrentTime;
    }
}
