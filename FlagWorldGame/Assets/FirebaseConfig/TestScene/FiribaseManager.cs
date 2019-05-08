using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
public class FiribaseManager : MonoBehaviour
{

    protected bool isFirebaseInitialized = false;

    ContactPointCollection myPointData = null;

    [SerializeField]
    private TextMeshProUGUI myFirstContactPointName = null;

    [SerializeField]
    Button continueButton = null;

    [SerializeField]
    private GameObject loadingIndicator = null;

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    public delegate void DatabaseError();
    public static event DatabaseError OnDatabaseError;


    void Start()
    {
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
                myFirstContactPointName.SetText(
                   "Could not resolve all Firebase dependencies: " + dependencyStatus);
                if(OnDatabaseError != null)
                    OnDatabaseError();
                
            }
        });
    }
    

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;

        if (!IsNetworkReachable())
        {

           SpawnDatabaseError();

        }
        else
        {
            RetryConnection();
        }
        isFirebaseInitialized = true;
    }

    private void SpawnDatabaseError() {
            myFirstContactPointName.SetText("Network not reacable");
            if(OnDatabaseError != null)
                OnDatabaseError();
            loadingIndicator.SetActive(false);
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
        public void RetryConnection() {
                //continue download proggress
            loadingIndicator.SetActive(true);
            StartCoroutine(checkInternetConnection((isConnected) =>
            {
                myFirstContactPointName.SetText($"Network state: {isConnected}");
                //set button to download or retry connectetion based on the network state
                if (isConnected)
                {
                    CheckDatabaseVersion();
                }
                else
                {
                    SpawnDatabaseError();
                }
            }));
    }

    public void BackToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    protected void CheckDatabaseVersion()
    {
        loadingIndicator.SetActive(true);
        continueButton.interactable = false;
        myFirstContactPointName.SetText("Version Chekking...");
        string currentDatabaseVersion = PlayerPrefs.GetString("database_version");
        FirebaseDatabase.DefaultInstance
        .GetReference("database_version")
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                myFirstContactPointName.SetText("Version check failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string verNumberString = snapshot.GetRawJsonValue();

                if (verNumberString != currentDatabaseVersion)
                {
                    myFirstContactPointName.SetText("newData found");
                    TriviaSaveLoadSystem.DeleteData();
                    PlayerPrefs.SetString("database_version", verNumberString);
                    DownloadDataFromDatabase();
                }
                else
                {
                    myFirstContactPointName.SetText("No new data available...");
                    loadingIndicator.SetActive(false);
                    // retryConnectionPanel.SetActive(false);
                    myPointData = TriviaSaveLoadSystem.LoadContactPoints();
                    if(myPointData !=null) {
                        myFirstContactPointName.SetText("Trivia is up-to-date");
                        continueButton.interactable = true;

                    }
                }
            }
        });
    }

    public void SetUpContactPoints()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    protected void DownloadDataFromDatabase()
    {
        myFirstContactPointName.SetText("Download start...");
        FirebaseDatabase.DefaultInstance
        .GetReference("finnish_language")
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                myFirstContactPointName.SetText("Download failed");
            }
            else if (task.IsCompleted)
            {
                myFirstContactPointName.SetText("Download completed...");
                myFirstContactPointName.SetText("Saving to disk ...");
                DataSnapshot snapshot = task.Result;
                ContactPointCollection contactPoints = new ContactPointCollection();
                JsonUtility.FromJsonOverwrite(snapshot.GetRawJsonValue(), contactPoints);
                TriviaSaveLoadSystem.SaveContactPoints(contactPoints);
                myFirstContactPointName.SetText("Saving completed ...");
                continueButton.interactable = true;
                loadingIndicator.SetActive(false);
            }
        });
    }
}