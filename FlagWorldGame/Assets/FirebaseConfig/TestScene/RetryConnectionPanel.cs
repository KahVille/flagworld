using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryConnectionPanel : MonoBehaviour
{

    FiribaseManager firebaseManager;

    private ModalPanel modalPanel;

    public void CreateErrorPanel (string title = null, string description = null) {
        EventButtonDetails button1Detail = new EventButtonDetails {buttonTitle = "Retry", action = RetryConnection};
        EventButtonDetails button2Detail = new EventButtonDetails {buttonTitle = "Back to Title", action = ClosePanel};
        //two buttons and image
        SpawnPanel(title,description, button1Detail,button2Detail);
    }

    void SpawnPanel(string mainTitle, string titleDescription, EventButtonDetails button1, EventButtonDetails button2 = null, Sprite icon = null ) {
         ModalPanelDetails modalPanelDetails = new ModalPanelDetails {question = mainTitle,description = titleDescription, iconImage = icon};
          modalPanelDetails.button1Details = button1;
          modalPanelDetails.button2Details = button2;
          modalPanel.SpawnWithDetails (modalPanelDetails);
    }

    //grab instance of panel
    void Awake () {
        modalPanel = ModalPanel.Instance();
    }

    private void OnEnable() {
        FiribaseManager.OnDatabaseError += ShowPanel;
        firebaseManager = Object.FindObjectOfType <FiribaseManager>();
    }

    private void OnDisable() {
        FiribaseManager.OnDatabaseError -= ShowPanel;
    }

    public void ShowPanel(string title = null, string desc=null) {
        EventButtonDetails button1Detail = new EventButtonDetails {buttonTitle = "Retry", action = RetryConnection};
        EventButtonDetails button2Detail = new EventButtonDetails {buttonTitle = "Back to Title", action = ClosePanel};
        //two buttons and image
        SpawnPanel(title,desc, button1Detail,button2Detail);
    }

    public void ClosePanel() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void RetryConnection() {
            if(firebaseManager != null)
                firebaseManager.RetryConnection();
    }
}
