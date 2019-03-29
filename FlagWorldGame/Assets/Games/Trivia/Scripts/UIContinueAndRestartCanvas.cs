using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIContinueAndRestartCanvas : MonoBehaviour
{

    public void OnContinueButtonPressed() {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnRestartButtonPressed() {


        string sceneToLoad = "MainMenu";

        switch (PlayerPrefs.GetInt("CurrentLocationIdentifier", 0))
        {

            case 0:
            sceneToLoad = "Memory";
            break;

            case 1:
            sceneToLoad = "Sorting";
            break;

            case 2:
            sceneToLoad = "GoneWithTheWind";
            break;

            case 3:
            sceneToLoad = "EarthMap";
            break;
            
            default:
                sceneToLoad = "MainMenu";
            break;
        }

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

}
