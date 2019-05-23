using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager Instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    public delegate void LanguageLocalization();
    public static event LanguageLocalization OnLanguageLocalization;


    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        if(PlayerPrefs.GetInt("Language") == ((int) LanguageUtility.Language.Finnish)) {
                    LoadLocale("localizedText_fi.json");
        }
        else {
              LoadLocale("localizedText_en.json");
        }

    }

    public void LoadLocale(string localeFile = null) {
         StartCoroutine(LoadLocalizedText(localeFile));
    }

    IEnumerator LoadLocalizedText(string fileName)
    {

        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_ANDROID || UNITY_IOS
        using (UnityWebRequest webRequest = UnityWebRequest.Get(filePath))
        {

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError) {
            Debug.Log(webRequest.error);
        }
        else {
            string dataAsJson = webRequest.downloadHandler.text;
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++) 
            {
                localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);   
            }
            Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");
            if(OnLanguageLocalization !=null) {
                OnLanguageLocalization();
            }

        }

        }
        
#elif UNITY_EDITOR

        if (File.Exists (filePath)) {
            string dataAsJson = File.ReadAllText (filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++) 
            {
                localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);   
            }

            Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");
            if(OnLanguageLocalization !=null)
                OnLanguageLocalization();
        } else 
        {
            Debug.LogError ("Cannot find file!");
        }
#endif

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

}