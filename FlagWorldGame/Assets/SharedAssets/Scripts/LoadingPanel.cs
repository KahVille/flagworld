using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{

    [SerializeField]
    GameObject eventSystem = null;

    private void OnEnable() {
        eventSystem.SetActive(false);
        
    }
    
    private void OnDisable() {
        eventSystem.SetActive(true);
    }
}
