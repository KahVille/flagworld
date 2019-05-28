using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the state management of sub canvases

public class UITriviaCanvas : MonoBehaviour
{
    
    [SerializeField]
    private GameObject loadingIndicator = null;

    [SerializeField]
    private GameObject eventSystem = null;

    [SerializeField]
    private GameObject questions = null;

    [SerializeField]
    private Canvas contiueAndRestartCanvas = null;

    public void ShowContiueAndRestartCanvas(int roundScore, int numberOfQuestions)
    {
        contiueAndRestartCanvas.enabled = true;
        contiueAndRestartCanvas.GetComponent<UIContinueAndRestartCanvas>().SetTriviaScoreText(roundScore,numberOfQuestions);
    }

    public void EnableLoadIndicator() {
        loadingIndicator.SetActive(true);
    }

    public void DisableLoadIndicator() {
        loadingIndicator.SetActive(false);
    }

    public void EnableEventSystem() {
        eventSystem.SetActive(true);
    }
    public void DisableEventSystem() {
        eventSystem.SetActive(false);
    }

    public void EnableQuestionCanvas() {
        questions.SetActive(true);
    }
    public void DisableQuestionCanvas() {
        questions.SetActive(false);
    }

}
