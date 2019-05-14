// This script manages a lot of audio related stuff e.g. volume.

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void Mute()
    {
        float vol;
        masterMixer.GetFloat("MasterVolume", out vol);
        if(vol < -10f)
        {
            masterMixer.SetFloat("MasterVolume", 0.0f);
        }
        else
        {
            masterMixer.SetFloat("MasterVolume", -80.0f);
        }
    }
}
