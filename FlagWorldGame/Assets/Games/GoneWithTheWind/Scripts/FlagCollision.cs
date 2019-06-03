﻿// Detects collision with the flag object.

using UnityEngine;

public class FlagCollision : MonoBehaviour
{
    PlayerStuffGWTW playerScript;

    private void Start() 
    {
        playerScript = FindObjectOfType<PlayerStuffGWTW>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.transform.CompareTag("Bird"))
        {
            playerScript.TimeDamage(5.0f);
            playerScript.Damage(100.0f); 
        }
        if(other.transform.CompareTag("Wind"))
        {
            playerScript.Windy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.transform.CompareTag("Wind"))
        {
            playerScript.Windy = false;
        }   
    }
}
