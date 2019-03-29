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
    Location lastLocation;
    public Location[] locations;
    float lastDistance;

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
        //autologImg.color = Color.red;

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
            //debugText.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
			debugText.text = "Working, yay!";
            tracking = true;
            lastDistance = float.MaxValue;
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

    void CheckClosestLoc()
    {
        float tempDist;
        for(int i = 0; i < locations.Length; i++)
        {
            tempDist = Calculate_Distance(longitude, latitude, locations[i].longitude, locations[i].latitude);
            if(tempDist < lastDistance)
            {
                lastDistance = tempDist;
                lastLocation = locations[i];
            }

            // Check if distance is within a locations range, if it is, change the image color.
            if(lastDistance < locations[i].rangeDistance)
            {
                locations[i].image.color = Color.green;
            }
            else if(lastDistance > locations[i].rangeDistance)
            {
                locations[i].image.color = Color.red;
            }
        }
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
            CheckClosestLoc();
            // if(lastDistance < rangeDistance)
            // {
            //     debugText.text = "IN " + lastLocation.name;
            // }
            // else
            // {
            //     debugText.text = "Dist: " + lastDistance;
            // }
            if(lastDistance < lastLocation.rangeDistance)
            {
                debugText.text = "IN " + lastLocation.name;
                SetCurrentContactPointForTrivia(lastLocation.name);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Trivia",UnityEngine.SceneManagement.LoadSceneMode.Single);

            }
            else
            {
                debugText.text = "Dist: " + lastDistance;
            }
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

    private void SetCurrentContactPointForTrivia(string currentLocationName = null) {
        

        //identified might be modified in the future to be more descriptive. like HAM473, or RaatiH123
        switch (currentLocationName)
        {
            case "Koulu":
            PlayerPrefs.SetInt("CurrentLocationIdentifier", 2);
            break;
            case "Suomenlipuntie":
            PlayerPrefs.SetInt("CurrentLocationIdentifier", 0);
            break;
            case "Lipputorni":
            PlayerPrefs.SetInt("CurrentLocationIdentifier", 1);
            break;
            case "Suurlippu":
            PlayerPrefs.SetInt("CurrentLocationIdentifier", 2);
            break;
            case "Raatihuone":
            PlayerPrefs.SetInt("CurrentLocationIdentifier", 3);
            break;   
            
            default:
            break;
        }

    }

    private void OnDestroy() 
    {
        Input.location.Stop();
    }

    // GPS functions
    // https://answers.unity.com/questions/1221259/how-to-get-distance-from-2-locations-with-unity-lo.html
	float DegToRad(float deg)
	{
		float temp;
		temp = (deg * Mathf.PI) / 180.0f;
		temp = Mathf.Tan(temp);
		return temp;
	}
 
  	float Distance_x(float lon_a, float lon_b, float lat_a, float lat_b)
	{
		float temp;
		float c;
		temp = (lat_b - lat_a);
		c = Mathf.Abs(temp * Mathf.Cos((lat_a + lat_b)) / 2);
		return c;
	}
 
	private float Distance_y(float lat_a, float lat_b)
	{
		float c;
		c = (lat_b - lat_a);
		return c;
	}
 
	float Final_distance(float x, float y)
	{
		float c;
		c = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f))) * 6371;
		return c;
	}
 
     //*******************************
 //This is the function to call to calculate the distance between two points
 
	public float Calculate_Distance(float long_a, float lat_a,float long_b, float lat_b )
	{
		float a_long_r, a_lat_r, p_long_r, p_lat_r, dist_x, dist_y, total_dist;
		a_long_r =DegToRad(long_a);
		a_lat_r = DegToRad(lat_a);
		p_long_r = DegToRad(long_b);
		p_lat_r = DegToRad(lat_b);
		dist_x = Distance_x(a_long_r, p_long_r, a_lat_r, p_lat_r);
		dist_y = Distance_y(a_lat_r, p_lat_r);
		total_dist = Final_distance(dist_x, dist_y);
		//total_dist = total_dist / 3.6917f; // I got this value by testing, this should give us kilometers        
		return total_dist; 
	}
}
