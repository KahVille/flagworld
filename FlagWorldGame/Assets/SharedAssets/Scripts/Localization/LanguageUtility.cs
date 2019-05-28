using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageUtility
{

    public enum Language
    {
        Finnish,
        English
    }

    public static int GetCurrentLanguage() {
        return PlayerPrefs.GetInt("Language");
    }

    public static void SetCurrentLanguage(int currentLaguage) {
        PlayerPrefs.SetInt("Language", currentLaguage);
    }

}
