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
    public string description; // Not implemented
    public string question;
    public Sprite iconImage;
    public Sprite panelBackgroundImage; // Not implemented
    public EventButtonDetails button1Details;
    public EventButtonDetails button2Details;
}


public class ModalPanel : MonoBehaviour {
    
    public TextMeshProUGUI description;
    public TextMeshProUGUI question;
    public Image iconImage;
    public Button button1;
    public Button button2;

    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    
    public GameObject modalPanelObject;
    
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
        modalPanelObject.SetActive (true);

        this.iconImage.gameObject.SetActive(false);
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);

        this.question.text = details.question;

        if(details.description != null || details.description != "")
        this.description.text = details.description;

        if (details.iconImage) {
            this.iconImage.sprite = details.iconImage;
            this.iconImage.gameObject.SetActive(true);
        }

        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener (details.button1Details.action);
        button1.onClick.AddListener (ClosePanel);
        button1Text.text = details.button1Details.buttonTitle;
        button1.gameObject.SetActive(true);
        
        if (details.button2Details != null) {
            button2.onClick.RemoveAllListeners();
            button2.onClick.AddListener (details.button2Details.action);
            button2.onClick.AddListener (ClosePanel);
            button2Text.text = details.button2Details.buttonTitle;
            button2.gameObject.SetActive(true);
        }
    }
        
    void ClosePanel () {
        modalPanelObject.SetActive (false); 
    }
}
