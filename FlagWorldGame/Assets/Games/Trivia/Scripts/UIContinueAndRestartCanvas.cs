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
        SceneManager.LoadScene("Trivia", LoadSceneMode.Single);
    }

}
