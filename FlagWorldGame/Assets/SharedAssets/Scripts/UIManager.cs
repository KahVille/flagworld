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
        SceneManager.LoadScene("GPSTest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Delete all High score keys
    public void DeleteAllHighscores()
    {
        PlayerPrefs.DeleteKey("GWTWHigh");
        PlayerPrefs.DeleteKey("SortingHigh");
        PlayerPrefs.DeleteKey("MemoryHigh");
        PlayerPrefs.DeleteKey("ConnectHigh");
    }

    // Delete gwtw playerpref key
    public void DeletThis()
    {
        PlayerPrefs.DeleteKey("GWTWHigh");
    }
}
