using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
    bool autologging;
    public Image autologImg;
    string serializedData;
    int logIndex;

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
        autologging = false;
        autologImg.color = Color.red;

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

        if(!File.Exists(Application.persistentDataPath + "/GPSDatas.txt"))
        {
            File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", String.Empty);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit(); 
        }
    }

    public void ToggleAutolog()
    {
        autologging = !autologging;
        if(autologging)
        {
            autologImg.color = Color.green;
        }
        else
        {
            autologImg.color = Color.red;
        }
    }

    public void WriteToTxt()
    {
        serializedData = logIndex.ToString() + ":" + "\n"
        + "Longitude: " + longitude.ToString() + "\n"
        + "Latitude: " + latitude.ToString() + "\n" + "\n";
        File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", serializedData);
        logIndex++;
    }

    public void ClearTxt()
    {
        File.WriteAllText(Application.persistentDataPath + "/GPSDatas.txt", String.Empty);
    }

    IEnumerator UpdateLocation()
    {
        DateTime now = DateTime.Now;
        File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", "-+-+-+-+-" + now.ToString("F") + "-+-+-+-+-"  +"\n");
        logIndex = 0;
        while(tracking)
        {
            longitude = Input.location.lastData.longitude;
			latitude = Input.location.lastData.latitude;
            longText.text = "Longitude: " + longitude.ToString();
            latText.text = "Latitude: " + latitude.ToString();

            if(autologging)
            {
                serializedData = logIndex.ToString() + ":" + "\n"
                + "Longitude: " + longitude.ToString() + "\n"
                + "Latitude: " + latitude.ToString() + "\n" + "\n";
                File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", serializedData);
                logIndex++;
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
