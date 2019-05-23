using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LanguageSelectionCanvas : MonoBehaviour
{

LocalizationManager localeManager = null;


    [SerializeField]
    Image[] selectedColors = null;

    int originalLanguage;

    [SerializeField]
    GameObject firebasePrefab = null;


    private void Awake() {

        originalLanguage = LanguageUtility.GetCurrentLanguage();
        selectedColors[LanguageUtility.GetCurrentLanguage()].enabled = true;
        if(PlayerPrefs.GetInt("FirstTimeCompleted") != 1) {
            GetComponent<Canvas>().enabled = true;
        }

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
    }

    public void ShowLanguageSelectionCanvas() {
        transform.SetAsLastSibling();
        GetComponent<Canvas>().enabled = true;
    }


    public void ConfirmSelection()
    {

        if ((originalLanguage == LanguageUtility.GetCurrentLanguage()) && (PlayerPrefs.GetInt("FirstTimeCompleted") == 1))
        {
            GetComponent<Canvas>().enabled = false;
            return;
        }

        PlayerPrefs.SetInt("FirstTimeCompleted", 1);
        originalLanguage = LanguageUtility.GetCurrentLanguage();
        CreateAndEnableFirebaseInstance();
        GetComponent<Canvas>().enabled = false;
    }

    private void CreateAndEnableFirebaseInstance()
    {
        FirebaseManager firebase = FindObjectOfType<FirebaseManager>() as FirebaseManager;
        if (firebase == null)
        {
            Instantiate(firebasePrefab);
            firebase = FindObjectOfType<FirebaseManager>() as FirebaseManager;
        }
        //delete database version player pref
        PlayerPrefs.DeleteKey("database_version");
        firebase.CheckDatabaseVersion();
    }
}
