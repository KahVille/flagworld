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

    bool isNetworkReached = false;

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
                continueButton.interactable = true;
                loadingIndicator.SetActive(false);
            }
        });
    }


    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                isNetworkReached = false;
               yield return false;
            }
            else
            {
                isNetworkReached = true;
                yield return true;
            }
        }
    }


    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        if(IsNetworkReachable()) {
            CheckDatabaseVersion();
        }
        else {
            myFirstContactPointName.SetText("Network not reacable");
        }
        isFirebaseInitialized = true;
    }

    private bool IsNetworkReachable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        return true;
    }

    protected void CheckDatabaseVersion()
    {
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
                    continueButton.interactable = true;
                    loadingIndicator.SetActive(false);
                }
            }
        });

    }

    public void SetUpContactPoints()
    {
        myPointData = TriviaSaveLoadSystem.LoadContactPoints();
        myFirstContactPointName.SetText("Load completed...");
        if (myPointData != null)
        {
            myFirstContactPointName.SetText(myPointData.points[0].questions[0].questionText);
        }
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