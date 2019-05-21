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
    }

    private void OnDisable() {
        LocalizationManager.OnLanguageLocalization -= GetLocalizedText;
    }

    private void Start() {
       GetLocalizedText();
    }

    public void GetLocalizedText()
    {
        string key_value = LocalizationManager.Instance.GetLocalizedValue(key);
        text.SetText(key_value);
    }
}
