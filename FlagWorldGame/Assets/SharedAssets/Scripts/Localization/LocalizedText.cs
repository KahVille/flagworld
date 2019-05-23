using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LocalizedText : MonoBehaviour, ILocalizedText {

    [SerializeField]
    private string key = "";

     TextMeshProUGUI text = null;

	// Use this for initialization
	void Awake () {
        text = GetComponent<TextMeshProUGUI>();
	}

    private void OnEnable() {
        LocalizationManager.OnLanguageLocalization += GetLocalizedText;
        GetLocalizedText();
    }

    private void OnDisable() {
        LocalizationManager.OnLanguageLocalization -= GetLocalizedText;
    }

    public void GetLocalizedText()
    {
        if(LocalizationManager.Instance !=null){
        string key_value = LocalizationManager.Instance.GetLocalizedValue(key);
        text.SetText(key_value);
        }
    }
}
