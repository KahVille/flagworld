using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using TMPro;
using UnityEngine.Networking;

//fixes ui hang with firebase
//https://forum.unity.com/threads/can-only-be-called-from-the-main-thread.622948/
// Add System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext()


public class FirebaseManager : MonoBehaviour
{



    [SerializeField]
    private GameObject loadingIndicator = null;

    [SerializeField]
    Sprite networkErrorSprite = null;

    private ModalPanel modalPanel;

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    ContactPointCollection myPointData = null;





    //grab instance of panel
    void Awake()
    {
        modalPanel = ModalPanel.Instance();
    }

    void Start()
    {
        StartFirebase();
    }

    void SpawnPanel(string mainTitle, string titleDescription, EventButtonDetails button1, EventButtonDetails button2 = null, Sprite icon = null)
    {
        ModalPanelDetails modalPanelDetails = new ModalPanelDetails { shortText = mainTitle, description = titleDescription, iconImage = icon };
        modalPanelDetails.button1Details = button1;
        modalPanelDetails.button2Details = button2;
        modalPanel.SpawnWithDetails(modalPanelDetails);
    }

    //ErrorDisplays
    void ShowNetworkError()
    {
        string buttonRetryText = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("try_again_button") : " Try Again";
        string networkTextShort = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("Network_error_short") : " Network error";
        string networkTextLong = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("Network_error_long") : " please enable network";
        loadingIndicator.SetActive(false);
        EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = buttonRetryText, action = RetryConnection };
        SpawnPanel(networkTextShort, networkTextLong, button1Detail, null, networkErrorSprite);
    }

    void ShowFirebaseError()
    {
        loadingIndicator.SetActive(false);
        EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = "Reload", action = ReloadScene };
        SpawnPanel("Firebase Error", "please Reload the game", button1Detail);
    }

    //callbacks for buttons
    void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


    void ContinueSuccess() { }

    //end of callbacks

    public void StartFirebase()
    {
#if !UNITY_EDITOR

        if(PlayerPrefs.GetInt("FirstTimeCompleted") == 1){
        loadingIndicator.SetActive(true);
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                ShowFirebaseError();
            }
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());

#elif UNITY_EDITOR
        loadingIndicator.SetActive(false);
        EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = "OK I understand", action = ContinueSuccess };
        SpawnPanel("You run in Editor", "in case of null refrence exception in trivia, please test on a mobile device", button1Detail, null, networkErrorSprite);
#endif
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;

        if (!IsNetworkReachable())
        {
            ShowNetworkError();
        }
        else
        {
            loadingIndicator.SetActive(false);
            if(PlayerPrefs.GetInt("FirstTimeCompleted") == 1){
                RetryConnection();
            }
        }
    }

    IEnumerator checkInternetConnection(System.Action<bool> action)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://firebase.google.com/"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                action(false);
            }
            else
            {
                action(true);
            }
        }
    }

    private bool IsNetworkReachable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        return true;
    }
    public void RetryConnection()
    {
#if !UNITY_EDITOR
        //continue download proggress
        loadingIndicator.SetActive(true);
        StartCoroutine(checkInternetConnection((isConnected) =>
        {
            //set button to download or retry connectetion based on the network state
            if (isConnected)
            {
                CheckDatabaseVersion();
            }
            else
            {
                ShowNetworkError();
            }
        }));
#endif
    }

    public void DownloadWithNewLanguage() {
        #if !UNITY_EDITOR
        loadingIndicator.SetActive(true);
        if (!IsNetworkReachable())
        {
            ShowNetworkError();
            return;
        }
        #endif

        if(PlayerPrefs.GetInt("Language") == ((int) LanguageUtility.Language.Finnish)) {
            DownloadDataFromDatabase("finnish_language");
        }
        else {
            DownloadDataFromDatabase("english_language");
        }
    }

    public void CheckDatabaseVersion()
    {
#if !UNITY_EDITOR
        loadingIndicator.SetActive(true);
        string currentDatabaseVersion = PlayerPrefs.GetString("database_version");
        FirebaseDatabase.DefaultInstance
        .GetReference("database_version")
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string verNumberString = snapshot.GetRawJsonValue();

                if (verNumberString != currentDatabaseVersion)
                {
                    TriviaSaveLoadSystem.DeleteData();
                    PlayerPrefs.SetString("database_version", verNumberString);
                    //TODO: Implement a function that checks current selected language, possible to do with the localization manager or playerprefabs.
                    DownloadWithNewLanguage();
                   
                }
                else
                {
                    myPointData = TriviaSaveLoadSystem.LoadContactPoints();
                    if (myPointData != null)
                    {
                        //trivia up to date
                        string buttonContinueText = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("continue_button") : "Continue";
                        string triviaUpToDate = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("trivia_up_to_date") : "Trivia up to date";
                        loadingIndicator.SetActive(false);
                        EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = buttonContinueText, action = ContinueSuccess };
                        SpawnPanel(triviaUpToDate, "", button1Detail);
                    }
                }
            }
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
#endif
    }

    protected void DownloadDataFromDatabase(string selectedLanguage = null)
    {
#if !UNITY_EDITOR
        FirebaseDatabase.DefaultInstance
        .GetReference(selectedLanguage)
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                string buttonContinueText = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("continue_button") : "Continue";
                string triviaUpToDate = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("trivia_up_to_date") : "Trivia up to date";
                string newDataDownloaded = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.GetLocalizedValue("trivia_data_long") : " Trivia new data downloaded";
                DataSnapshot snapshot = task.Result;
                ContactPointCollection contactPoints = new ContactPointCollection();
                JsonUtility.FromJsonOverwrite(snapshot.GetRawJsonValue(), contactPoints);
                TriviaSaveLoadSystem.SaveContactPoints(contactPoints);
                loadingIndicator.SetActive(false);
                EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = buttonContinueText, action = ContinueSuccess };
                SpawnPanel(triviaUpToDate, newDataDownloaded, button1Detail);
            }
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
#endif
    }
}