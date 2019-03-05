using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class GPSScript : MonoBehaviour
{
    public TextMeshProUGUI longText;
    public TextMeshProUGUI latText;
    public TextMeshProUGUI debugText;
    bool tracking;
    float longitude;
    float latitude;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        debugText.text = "Start";
        
        // Android permissions
        #if UNITY_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        #endif

        yield return new WaitForSeconds(5f);
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
		{
			debugText.text = "Not Enabled";
            yield break;
		}
        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            debugText.text = "Timed out";
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            debugText.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            debugText.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
			tracking = true;
            StartCoroutine(UpdateLocation());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit(); 
        }
    }

    IEnumerator UpdateLocation()
    {
        string serializedData;
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/GPSDatas.txt", true);
        DateTime now = DateTime.Now;
        writer.Write("-+-+-+-+-" + now.ToString("F") + "-+-+-+-+-"  +"\n");
        int index = 0;
        while(tracking)
        {
            longitude = Input.location.lastData.longitude;
			latitude = Input.location.lastData.latitude;
            longText.text = "Longitude: " + longitude.ToString();
            latText.text = "Latitude: " + latitude.ToString();

            serializedData = index.ToString() + ":" + "\n"
            + "Longitude: " + longitude.ToString() + "\n"
            + "Latitude: " + latitude.ToString() + "\n" + "\n";
            writer.Write(serializedData);

            yield return new WaitForSeconds(3f);
        }
        writer.Close();
    }
}
