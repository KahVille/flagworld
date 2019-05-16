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

    ContactPointCollection myPointData = null;

    [SerializeField]
    private GameObject loadingIndicator = null;

    //for firebase ui to work
    //https://forum.unity.com/threads/can-only-be-called-from-the-main-thread.622948/
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public delegate void DatabaseError(string title, string description);
    public static event DatabaseError OnDatabaseError;


    void Start()
    {
        StartFirebase();
    }
    

    private void StartFirebase() {
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
                if(OnDatabaseError != null)
                    OnDatabaseError("Firebase not init","");
            }
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext() );
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;

        if (!IsNetworkReachable())
        {
            if(OnDatabaseError != null) {
                loadingIndicator.SetActive(false);
                 OnDatabaseError("network not reached","please try again");
            }
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
        public void RetryConnection() {
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
                   if(OnDatabaseError != null) {
                       loadingIndicator.SetActive(false);
                        OnDatabaseError("network not reached","please try again");
                   }

                }
            }));
    }

    protected void CheckDatabaseVersion()
    {
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
                    loadingIndicator.SetActive(false);
                    myPointData = TriviaSaveLoadSystem.LoadContactPoints();
                    if(myPointData !=null) {
                        if(OnDatabaseError != null) {
                       loadingIndicator.SetActive(false);
                        OnDatabaseError("trivia up-to-date","please continue");
                   }
                    }
                }
            }
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void HideDatabaseConnectionCheck() {
        gameObject.SetActive(false);
    }

    protected void DownloadDataFromDatabase(string selectedLanguage = null)
    {
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
                if(OnDatabaseError != null) {
                       loadingIndicator.SetActive(false);
                        OnDatabaseError("Dowloaded the newest data","please continue");
                   }
            }
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext() );
    }
}