using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LocalizedText : MonoBehaviour, ILocalizedText {

    [SerializeField]
    private string key = "";

     TextMeshProUGUI text = null;

     LocalizationManager localManager = null;

	// Use this for initialization
	void Awake () {
        text = GetComponent<TextMeshProUGUI>();
        localManager = LocalizationManager.Instance;
	}

    private void Start() {
       GetLocalizedText();
    }

    public void GetLocalizedText()
    {
        if(localManager !=null) {
        string key_value = LocalizationManager.Instance.GetLocalizedValue(key);
            if(key_value !=null) {
                text.SetText(key_value);
            }

        }

    }
}
