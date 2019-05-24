using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void RestartLevel()
    {
        PlayerPrefs.SetInt("CurrentGameHigh", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        PlayerPrefs.SetInt("CurrentGameHigh", 0);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Delete gwtw playerpref key
    public void DeletThis()
    {
        PlayerPrefs.DeleteKey("GWTWHigh");
    }
}
