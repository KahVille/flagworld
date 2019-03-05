using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomDebug : MonoBehaviour
{
    public void NextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCount)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
