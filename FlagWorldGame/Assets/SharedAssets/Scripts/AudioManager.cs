// This script manages a lot of audio related stuff e.g. volume.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public ButtonImageToggle[] buttonImgToggles;

    private void Start() 
    {
        if(!PlayerPrefs.HasKey("Mute"))
        {
            PlayerPrefs.SetInt("Mute", 1);
        }    
        if(SceneManager.GetActiveScene().name == "GPSTest")
        {
            bool isMuted = PlayerPrefs.GetInt("Mute") > 0 ? true : false;
            buttonImgToggles[0].SetImg(isMuted);
            Mute(isMuted);
        }
        else
        {
            bool isMuted = PlayerPrefs.GetInt("Mute") > 0 ? true : false;
            Mute(isMuted);
        }
    }

    public void Mute()
    {
        float vol;
        masterMixer.GetFloat("MasterVolume", out vol);
        if(vol < -10f)
        {
            masterMixer.SetFloat("MasterVolume", 0.0f);
            PlayerPrefs.SetInt("Mute", 0);
        }
        else
        {
            masterMixer.SetFloat("MasterVolume", -80.0f);
            PlayerPrefs.SetInt("Mute", 1);
        }
    }

    public void Mute(bool muted)
    {
        if(!muted)
        {
            masterMixer.SetFloat("MasterVolume", 0.0f);
        }
        else
        {
            masterMixer.SetFloat("MasterVolume", -80.0f);
        }
    }
}
