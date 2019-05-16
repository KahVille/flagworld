using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class UIContinueAndRestartCanvas : MonoBehaviour
{


    [SerializeField]
    TextMeshProUGUI endText = null;


    public void OnContinueButtonPressed() {
        SceneManager.LoadScene("GPSTest", LoadSceneMode.Single);
    }

    public void SetTriviaScoreText(int roundScore, int numberOfQuestions) {
            endText.SetText($"Round Ended, You answered correctly to: {roundScore} out of {numberOfQuestions} questions");
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
