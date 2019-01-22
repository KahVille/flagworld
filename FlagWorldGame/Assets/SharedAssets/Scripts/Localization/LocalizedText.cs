using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LocalizedText : MonoBehaviour, ILocalizedText {

    [SerializeField]
    private string key = "";

	// Use this for initialization
	void Start () {
        Text text = GetComponent<Text>();
        text.text = LocalizationManager.Instance.getLocalizedValue(key);
	}

    public void GetLocalizedText()
    {
        Text text = GetComponent<Text>();
        text.text = LocalizationManager.Instance.getLocalizedValue(key);
    }

    /*
     *
     *   List<GameObject> LocalizedObjects
         for (int i = 0; i < localizedObjects.Count; i++)
            {
                localizedObjects[i].GetComponent<ILocalizedText>().GetLocalizedText();
            }
     * 
     */
}
