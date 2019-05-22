using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirstTimeSetup : MonoBehaviour
{

LocalizationManager localeManager = null;


    [SerializeField]
    Image[] selectedColors = null;

    int originalLanguage;


    private void Awake() {
        selectedColors[LanguageUtility.GetCurrentLanguage()].enabled = true;
        originalLanguage = LanguageUtility.GetCurrentLanguage();
    }


    public void SetCurrentLanguage(int languageSelected) {
        if(languageSelected == LanguageUtility.GetCurrentLanguage()) {
            return;
        }
        
        localeManager = FindObjectOfType<LocalizationManager>() as LocalizationManager;
        LanguageUtility.Language selected = (LanguageUtility.Language) languageSelected;
        foreach (var item in selectedColors)
        {
            item.enabled = false;
        }

        switch (selected)
        {

            case LanguageUtility.Language.Finnish:
            selectedColors[0].enabled = true;
            PlayerPrefs.SetInt("Language", (int)selected);
            localeManager.LoadLocale("localizedText_fi.json");
            break;

            case LanguageUtility.Language.English:
            selectedColors[1].enabled = true;
            PlayerPrefs.SetInt("Language", (int)selected);
            localeManager.LoadLocale("localizedText_en.json");
            break;
            
            default:
            break;
        }

    PlayerPrefs.SetInt("FirstTimeCompleted",1);
    }


    public void ConfirmSelection() {

            if(originalLanguage == LanguageUtility.GetCurrentLanguage()) {
                    GetComponent<Canvas>().enabled = false;
                    return;
            }

            originalLanguage = LanguageUtility.GetCurrentLanguage();
            FirebaseManager firebase = FindObjectOfType<FirebaseManager>() as FirebaseManager;
            firebase.DownloadWithNewLanguage();
            GetComponent<Canvas>().enabled = false;
    }


}
