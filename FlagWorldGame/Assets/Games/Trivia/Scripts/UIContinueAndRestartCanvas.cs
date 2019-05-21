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
        string triviaEndText = LocalizationManager.Instance.GetLocalizedValue("game_end_text_trivia");
            endText.SetText($"{triviaEndText} \n {roundScore} / {numberOfQuestions}");
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
