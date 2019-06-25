// Script mainly in charge of changing the onclick listener on the exit button.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GPSButtonsScript : MonoBehaviour
{
    public Button exitBtn;
    public Button menuBackBtn;
    public Button optionsBackBtn;
    public Button menuBtn;
    public Button triviaBtn;
    public GameObject[] buttons;
    [SerializeField]
    Animator menuAnim = null;
    [SerializeField]
    Animator optionsAnim = null;
    CameraMovementGPS cameraMovementScript;
    

    private void Start()
    {
        // Delegate listener. BackToMainMenu() on exitBtn.
        // exitBtn.onClick.AddListener(delegate { exitBtn.GetComponent<UIExitBtnScript>().BackToMainMenu(); });
        menuBtn.onClick.AddListener(delegate { FromBackToMainMenuToCloseMenu(); });
        cameraMovementScript = FindObjectOfType<CameraMovementGPS>();
    }

    public void HideOrShowButtons(bool hide)
    {
        if(hide)
        {
            foreach(GameObject obj in buttons)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach(GameObject obj in buttons)
            {
                obj.SetActive(true);
            }
        }
    }

    // Called when going back to map from the main menu
    public void FromCloseMenuToBackToMainMenu()
    {
        exitBtn.onClick.AddListener(delegate { exitBtn.GetComponent<UIExitBtnScript>().BackToMainMenu(); });
    }

    // Changes the exit button's functionality from changing the scene to main menu to closing the options menu.
    // This happens when menu is shown.
    public void FromBackToMainMenuToCloseMenu()
    {
        //exitBtn.GetComponent<Animator>().SetBool("Show", false);
        cameraMovementScript.CanMove = false;
        triviaBtn.gameObject.SetActive(false);
        //exitBtn.onClick.RemoveAllListeners();
        menuBackBtn.onClick.AddListener(delegate { menuBackBtn.GetComponent<UIExitBtnScript>().CloseMenu(menuAnim, triviaBtn); });
        menuAnim.SetBool("ShowPanel", true);
    }

    // Called when going back from submenu to menu
    public void FromSubToMain()
    {
        optionsBackBtn.onClick.RemoveAllListeners();
        menuBackBtn.onClick.AddListener(delegate { menuBackBtn.GetComponent<UIExitBtnScript>().CloseMenu(menuAnim, triviaBtn); });
    }


    // Need to give the options-panel animator for the exitBtn.
    public void FromCloseMenuToCloseOptions()
    {
        menuBackBtn.onClick.RemoveAllListeners();
        optionsBackBtn.onClick.AddListener(delegate { optionsBackBtn.GetComponent<UIExitBtnScript>().CloseMenu(optionsAnim, triviaBtn); });
        optionsAnim.SetBool("ShowPanel", true);
    }

    private void OnDisable() 
    {
        // exitBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.RemoveAllListeners();

    }

    public void ClickTrivia()
    {
        PlayerPrefs.SetInt("TriviaAllRandom", 1);
        SceneManager.LoadScene("Trivia");
    }
}
