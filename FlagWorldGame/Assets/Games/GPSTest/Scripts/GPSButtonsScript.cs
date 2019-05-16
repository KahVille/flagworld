// Script mainly in charge of changing the onclick listener on the exit button.

using UnityEngine;
using UnityEngine.UI;

public class GPSButtonsScript : MonoBehaviour
{
    public Button exitBtn;
    public Button menuBtn;
    [SerializeField]
    Animator menuAnim;
    [SerializeField]
    Animator optionsAnim;
    CameraMovementGPS cameraMovementScript;
    

    private void Start()
    {
        // Delegate listener. BackToMainMenu() on exitBtn.
        exitBtn.onClick.AddListener(delegate { exitBtn.GetComponent<UIExitBtnScript>().BackToMainMenu(); });
        menuBtn.onClick.AddListener(delegate { FromBackToMainMenuToCloseMenu(); });
        cameraMovementScript = FindObjectOfType<CameraMovementGPS>();
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
        cameraMovementScript.CanMove = false;
        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(delegate { exitBtn.GetComponent<UIExitBtnScript>().CloseMenu(menuAnim); });
        menuAnim.SetBool("ShowPanel", true);
    }

    // Called when going back from submenu to menu
    public void FromSubToMain()
    {
        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(delegate { exitBtn.GetComponent<UIExitBtnScript>().CloseMenu(menuAnim); });
    }


    // Need to give the options-panel animator for the exitBtn.
    public void FromCloseMenuToCloseOptions()
    {
        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(delegate { exitBtn.GetComponent<UIExitBtnScript>().CloseMenu(optionsAnim); });
        optionsAnim.SetBool("ShowPanel", true);
    }

    private void OnDisable() 
    {
        exitBtn.onClick.RemoveAllListeners();
        menuBtn.onClick.RemoveAllListeners();
    }
}
