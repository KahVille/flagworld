using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using TMPro;

public class GPSScript : MonoBehaviour
{
    public TextMeshProUGUI longText;
    public TextMeshProUGUI latText;
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI locationTitleText;
    public TextMeshProUGUI locationDescText;
    bool tracking;
    float longitude;
    float latitude;
    bool autologging;
    public Image autologImg;
    string serializedData;
    int logIndex;
    Location lastLocation;
    public Location[] locations;
    public List<Button> locBtns = new List<Button>();
    // Play-button in the pop-up menu
    public Button playBtn;
    float lastDistance;
    public GameObject infoPanelObj;
    Animator infoAnim;
    bool canOpenMenu;
    public float debugLongitude, debugLatitude;
    // Image, which shows are you close to the location on the pop-up panel
    public Image popupLocImg;
    public TextMeshProUGUI popupLocText;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        debugText.text = "Start";
        infoAnim = infoPanelObj.GetComponent<Animator>();
        canOpenMenu = false;
        
        // Android permissions
        #if UNITY_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        #endif

        #if UNITY_ANDROID && !UNITY_EDITOR
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

        #elif UNITY_EDITOR
        yield return null;
        debugText.text = "Working, yay!";
        tracking = true;
        lastDistance = float.MaxValue;
        StartCoroutine(UpdateLocation());
        #endif

        // Add listeners to location buttons
        for(int i = 0; i < locations.Length; i++)
        {
            int tmp = i;
            locBtns.Add(locations[i].image.transform.GetComponent<Button>());
            locBtns[i].onClick.AddListener(delegate {PushLocBtn(tmp);});
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Activate pop-up and set the correct texts
    public void PushLocBtn(int locationIndex)
    {
        Debug.Log(locationIndex);
        if(lastLocation != null)
            Debug.Log("LAST LOCATION: " + lastLocation.title);
        Debug.Log("LOCINDEX LOCATION: " + locations[locationIndex].title);

        // Open pop-up and show correct info
        infoPanelObj.GetComponent<DisableScript>().enabled = false;
        infoPanelObj.SetActive(true);
        infoAnim.SetBool("ShowPanel", true);
        locationTitleText.text = locations[locationIndex].title;
        locationDescText.text = locations[locationIndex].description;

        // Set listener for play button
        playBtn.onClick.AddListener(delegate {PushPlayBtn(locBtns[locationIndex].transform.name);});

        if(canOpenMenu && lastLocation != null && lastLocation.name == locations[locationIndex].name)
        {
            popupLocText.text = "Olet alueella";
            popupLocImg.color = Color.green;
        }
        else
        {
            popupLocText.text = "Et ole alueella";
            popupLocImg.color = Color.red;
        }
    }

    public void PushPlayBtn(string nameOfGame)
    {
        SceneManager.LoadScene(nameOfGame);
    }

    public void DisablePanel()
    {
        playBtn.onClick.RemoveAllListeners();
        infoAnim.SetBool("ShowPanel", false);
    }

    // Disable listeners
    private void OnDisable() 
    {
        for(int i = 0; i < locations.Length; i++)
        {
            locBtns[i].onClick.RemoveAllListeners();
        }

        playBtn.onClick.RemoveAllListeners();
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
        lastDistance = float.MaxValue;
        lastLocation = null;
        for(int i = 0; i < locations.Length; i++)
        {
            tempDist = Calculate_Distance(longitude, latitude, locations[i].longitude, locations[i].latitude);
            if(tempDist < lastDistance)
            {
                lastDistance = tempDist;
                lastLocation = locations[i];
            }
            //Debug.Log(lastDistance);
            locations[i].image.color = Color.red;

        }

        // Check if distance is within a locations range, if it is, change the image color.
        if(lastDistance < lastLocation.rangeDistance)
        {
            lastLocation.image.color = Color.green;
            canOpenMenu = true;
        }
        else
        {
            canOpenMenu = false;
        }
    }

    IEnumerator UpdateLocation()
    {
        DateTime now = DateTime.Now;
        File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", "-+-+-+-+-" + now.ToString("F") + "-+-+-+-+-"  +"\n");
        logIndex = 0;
        while(tracking)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
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

            }
            else
            {
                debugText.text = "Dist: " + lastDistance;
            }
            //longText.text = "Longitude: " + longitude.ToString();
            //latText.text = "Latitude: " + latitude.ToString();

            if(autologging)
            {
                serializedData = logIndex.ToString() + ":" + "\n"
                + "Longitude: " + longitude.ToString() + "\n"
                + "Latitude: " + latitude.ToString() + "\n" + "\n";
                File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", serializedData);
                logIndex++;
            }
            yield return new WaitForSeconds(3f);

            #elif UNITY_EDITOR
            longitude = debugLongitude;
            latitude = debugLatitude;
            CheckClosestLoc();
            if(lastDistance < lastLocation.rangeDistance)
            {
                debugText.text = "IN " + lastLocation.name;
            }
            else
            {
                debugText.text = "Dist: " + lastDistance;
            }
            yield return new WaitForSeconds(1f);

            #endif
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
