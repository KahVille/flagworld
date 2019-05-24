using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using TMPro;


/// <summary>
/// Button details with text and callback action
/// </summary>
/// <param name="action">Remember to assign a custom callback; action = YourCustomFunction</param>
public class EventButtonDetails {
    public string buttonTitle;
    public Sprite buttonBackground;  // Not implemented
    public UnityAction action;
}

public class ModalPanelDetails {
    public string description;
    public string shortText;
    public Sprite iconImage;
    public Sprite panelBackgroundImage; // Not implemented
    public EventButtonDetails button1Details;
    public EventButtonDetails button2Details;
}


public class ModalPanel : MonoBehaviour {



    [SerializeField]
    private ModalPanelCanvas modalPanelCanvas = null;
    
    private static ModalPanel modalPanel;
    
    public static ModalPanel Instance () {
        if (!modalPanel) {
            modalPanel = FindObjectOfType(typeof (ModalPanel)) as ModalPanel;
            if (!modalPanel)
                Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }
        return modalPanel;
    }
    
    public void SpawnWithDetails (ModalPanelDetails details){
        
        modalPanelCanvas.gameObject.SetActive (true);

        modalPanelCanvas.iconImage.gameObject.SetActive(false);
        modalPanelCanvas.button1.gameObject.SetActive(false);
        modalPanelCanvas.button2.gameObject.SetActive(false);

       modalPanelCanvas.shortText.text = details.shortText;

        if(details.description != null || details.description != "")
       modalPanelCanvas.description.text = details.description;

        if (details.iconImage) {
           modalPanelCanvas.iconImage.sprite = details.iconImage;
           modalPanelCanvas.iconImage.gameObject.SetActive(true);
        }

        modalPanelCanvas.button1.onClick.RemoveAllListeners();
        modalPanelCanvas.button1.onClick.AddListener (details.button1Details.action);
        modalPanelCanvas.button1.onClick.AddListener (ClosePanel);
        modalPanelCanvas.button1Text.text = details.button1Details.buttonTitle;
        modalPanelCanvas.button1.gameObject.SetActive(true);
        
        if (details.button2Details != null) {
            modalPanelCanvas.button2.onClick.RemoveAllListeners();
            modalPanelCanvas.button2.onClick.AddListener (details.button2Details.action);
            modalPanelCanvas.button2.onClick.AddListener (ClosePanel);
            modalPanelCanvas.button2Text.text = details.button2Details.buttonTitle;
            modalPanelCanvas.button2.gameObject.SetActive(true);
        }
    }
        
    void ClosePanel () {
         modalPanelCanvas.gameObject.SetActive (false); 
    }
}
