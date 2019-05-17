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


public class FiribaseManager : MonoBehaviour
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
        ModalPanelDetails modalPanelDetails = new ModalPanelDetails { question = mainTitle, description = titleDescription, iconImage = icon };
        modalPanelDetails.button1Details = button1;
        modalPanelDetails.button2Details = button2;
        modalPanel.SpawnWithDetails(modalPanelDetails);
    }

    //ErrorDisplays
    void ShowNetworkError()
    {
        loadingIndicator.SetActive(false);
        EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = "Retry", action = RetryConnection };
        SpawnPanel("Network Error", "please enable network connection", button1Detail, null, networkErrorSprite);
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

    private void StartFirebase()
    {
        #if !UNITY_EDITOR
        loadingIndicator.SetActive(true);
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
            RetryConnection();
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

    protected void CheckDatabaseVersion()
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
                    DownloadDataFromDatabase("finnish_language");
                }
                else
                {
                    myPointData = TriviaSaveLoadSystem.LoadContactPoints();
                    if (myPointData != null)
                    {
                        //trivia up to date
                        loadingIndicator.SetActive(false);
                        EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = "Continue", action = ContinueSuccess };
                        SpawnPanel("Trivia up-to-date", "please continue", button1Detail);
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
                DataSnapshot snapshot = task.Result;
                ContactPointCollection contactPoints = new ContactPointCollection();
                JsonUtility.FromJsonOverwrite(snapshot.GetRawJsonValue(), contactPoints);
                TriviaSaveLoadSystem.SaveContactPoints(contactPoints);
                loadingIndicator.SetActive(false);
                EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = "Continue", action = ContinueSuccess };
                SpawnPanel("Trivia up-to-date", "newest data downloaded", button1Detail);
            }
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        #endif
    }
}