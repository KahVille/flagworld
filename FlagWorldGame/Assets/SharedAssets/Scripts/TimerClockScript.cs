// Script responsible for updating radial clock

using UnityEngine;
using UnityEngine.UI;

public class TimerClockScript : MonoBehaviour
{
    // Put the radial image in this variable
    public Image radialImg;
    // Rect transform of the timer hand
    public RectTransform handRT;
    // Variable used to check if the clock should be updating
    bool isOn;
    float maxTime;
    float currentTime;
    public float currentRot;
    float maxRot;

    private void Start() 
    {
        if(!radialImg)
        {
            radialImg = transform.GetChild(1).GetComponent<Image>();
        }
        if(!handRT)
        {
            handRT = transform.GetChild(3).GetComponent<RectTransform>();
        }
    }

    private void Update() 
    {
        if(isOn)
        {
            radialImg.fillAmount = currentTime / maxTime;
            currentRot = (currentTime / maxTime) * maxRot;
            handRT.eulerAngles = new Vector3(0.0f, 0.0f, currentRot);
        }
    }

    public void InitializeClock(float newMaxTime)
    {
        maxTime = newMaxTime;
        currentTime = maxTime;
        maxRot = 360f;
        currentRot = maxRot;
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
