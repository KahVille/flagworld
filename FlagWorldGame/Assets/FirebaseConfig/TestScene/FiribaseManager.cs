using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;

public class FiribaseManager : MonoBehaviour
{

    protected bool isFirebaseReady = false;

    ContactPointCollection myPointData = null;

    [SerializeField]
    private TextMeshProUGUI myFirstContactPointName = null;

    // Start is called before the first frame update
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
        // Create and hold a reference to your FirebaseApp,
        // where app is a Firebase.FirebaseApp property of your application class.
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // Set a flag here to indicate whether Firebase is ready to use by your app.
        isFirebaseReady = true;
        DownloadDataFromDatabase();
        myPointData = TriviaSaveLoadSystem.LoadContactPoints();
        myFirstContactPointName.SetText(myPointData.points[0].name);
            }
            else
            {
                isFirebaseReady = false;
                UnityEngine.Debug.LogError(System.String.Format(
        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        // Firebase Unity SDK is not safe to use here.
    }
        });
    }


    private void DownloadDataFromDatabase()
    {
        if (isFirebaseReady)
        {
            FirebaseDatabase.DefaultInstance
            .GetReference("finnish_language")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
        // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    ContactPointCollection contactPoints = new ContactPointCollection();
                    JsonUtility.FromJsonOverwrite(snapshot.GetRawJsonValue(), contactPoints);
                    TriviaSaveLoadSystem.DeleteData();
                    TriviaSaveLoadSystem.SaveContactPoints(contactPoints);

                }
            });
        }
    }

}
