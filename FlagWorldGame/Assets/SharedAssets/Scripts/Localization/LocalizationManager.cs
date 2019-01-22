using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager Instance;

    private Dictionary<string, string> localizedText;

    private bool isReady = false;
    private string missingTextString = "Localized Text not Found";
	// Use this for initialization
	void Awake () {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

	}

    public void loadLocalizedText(string fileName = null)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath,fileName);
        if(File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, Dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Canot find file");
        }

        isReady = true;

    }

    public string getLocalizedValue(string key)
    {
        string result = missingTextString;
        if(localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;

    }

    public bool getIsReady()
    {
        return isReady;
    }

}
