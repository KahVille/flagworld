using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryConnectionPanel : MonoBehaviour
{

    FiribaseManager firebaseManager;

    private void OnEnable() {
        FiribaseManager.OnDatabaseError += ShowPanel;
        firebaseManager = Object.FindObjectOfType <FiribaseManager>();
    }

    private void OnDisable() {
        FiribaseManager.OnDatabaseError -= ShowPanel;
    }

    public void ShowPanel() {
        GetComponent<Canvas>().enabled = true;
    }

    public void HidePanel() {
        GetComponent<Canvas>().enabled = false;
        RetryConnection();
    }

    public void RetryConnection() {
            if(firebaseManager != null)
                firebaseManager.RetryConnection();
    }
}
