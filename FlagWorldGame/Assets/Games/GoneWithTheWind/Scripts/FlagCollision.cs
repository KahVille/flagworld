// Detects collision with the flag object.

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
            playerScript.FlagHP += 20f;
            playerScript.flagHPSlider.value = playerScript.FlagHP;
            if(playerScript.FlagHP > 100f)
            {
                playerScript.StartGameOver();
            }
        }
    }
}
