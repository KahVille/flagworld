using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeSetup : MonoBehaviour
{


    public void SetCurrentLanguage(string languageSelected = null) {
        if(languageSelected == null) {
            return;
        }
            
        
        switch (languageSelected)
        {

            case "fin":
            PlayerPrefs.SetString("Language", languageSelected);
            localeManager.LoadLocale("localizedText_fi.json");
            break;

            case "eng":
            PlayerPrefs.SetString("Language", languageSelected);
            localeManager.LoadLocale("localizedText_en.json");
            break;
            
            default:
            break;
        }

    PlayerPrefs.SetInt("FirstTimeCompleted",1);
    }



    public void ConfirmSelection() {
            FirebaseManager firebase = FindObjectOfType<FirebaseManager>() as FirebaseManager;
            firebase.StartFirebase();
            LocalizationManager localeManager = FindObjectOfType<LocalizationManager>() as LocalizationManager;
            GetComponent<Canvas>().enabled = false;
    }


}
