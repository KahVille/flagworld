using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScript : MonoBehaviour
{
    // private void OnEnable() 
    // {
    //     gameObject.SetActive(false);        
    // }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
