// Script for the top right exit buttons, made because this is easier than doing onSceneLoad in the
// ReturnButtonScript.

using UnityEngine;
using UnityEngine.SceneManagement;

public class UIExitBtnScript : MonoBehaviour
{
    // Quits the game. Used in the first language select screen.
    public void QuitGame()
    {
        Application.Quit();
    }

    // Closes the menu with closing animation.
    public void CloseMenu(Animator menu)
    {
        if(menu.name != "MenuPanel")
        {
            // Need to delegate new onclick to close main options
            FindObjectOfType<GPSButtonsScript>().FromSubToMain();
        }
        else
        {
            FindObjectOfType<CameraMovementGPS>().CanMove = true;
            FindObjectOfType<GPSButtonsScript>().FromCloseMenuToBackToMainMenu();
        }
        menu.SetBool("ShowPanel", false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
