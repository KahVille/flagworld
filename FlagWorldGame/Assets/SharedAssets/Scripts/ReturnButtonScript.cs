using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButtonScript : MonoBehaviour
{
    static ReturnButtonScript instance = null;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
