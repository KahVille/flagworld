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
        string filePath;

#if (UNITY_ANDROID) && !UNITY_EDITOR
        filePath = Path.Combine(Application.streamingAssetsPath, fileName);

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

#elif (UNITY_IOS)
        filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        string result = "";
        if (filePath.Contains("://"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        }
        else
            result = System.IO.File.ReadAllText(filePath);

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(result);
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        if (OnLanguageLocalization != null)
        {
            OnLanguageLocalization();
        }

#elif UNITY_EDITOR
        filePath = Path.Combine(Application.streamingAssetsPath, fileName);

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
        yield return true;
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