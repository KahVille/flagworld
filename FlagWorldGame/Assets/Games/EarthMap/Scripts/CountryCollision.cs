using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryCollision : MonoBehaviour
{
    PlayerScore psScript;

    void Start() 
    {
        psScript = FindObjectOfType<PlayerScore>();
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     Debug.Log(other.name);
    //     if(other.transform.name == transform.name)
    //     {
    //         psScript.AddScore();
    //         Debug.Log("PSSCORE: " + psScript.FlagPoints);
    //     }
    //     else if(other.transform.name != transform.name)
    //     {
    //         Debug.Log("rip");
    //     }
    // }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     Debug.Log(other.name);
    //     if(other.transform.name == transform.name)
    //     {
    //         psScript.AddScore(-1);
    //         Debug.Log("PSSCORE: " + psScript.FlagPoints);
    //     }
    //     else if(other.transform.name != transform.name)
    //     {
    //         Debug.Log("rip");
    //     }
    // }
}
