// Script in charge of the GPS. Updates the player's location on the map.

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
    // Variable for checking if the GPS is initializing
    public bool IsInitializing
    {
        get
        {
            return isInitializing;
        }
        set
        {
            isInitializing = value;
        }
    }
    bool isInitializing;
    double longitude;
    double latitude;
    bool autologging;
    public Image autologImg;
    string serializedData;
    int logIndex;
    Location lastLocation;
    public Location[] locations;
    List<Button> locBtns = new List<Button>();
    // Play-button in the pop-up menu
    public Button playBtn;
    // Trivia-button in the pop-up menu
    public Button triviaBtn;
    double lastDistance;
    public GameObject infoPanelObj;
    Animator infoAnim;
    bool canOpenMenu;
    public double debugLongitude, debugLatitude;
    // Image, which shows are you close to the location on the pop-up panel
    public Image popupLocImg;
    public TextMeshProUGUI popupLocText;
    // The map image. Also used in CameraMovementGPS to get the image bounds 
    public Image mapImage;
    // Corners of the map image. Indexes: 0 = bottomleft, 1 = bottomright, 2 = topright, 3 = topleft
    public Vector3[] mapCorners = new Vector3[4];
    // Image which shows where the player is on the map
    public Image userIndicatorImg;
    // Longitudes and latitudes for the map images corners
    public double bottomLeftLongitude, bottomRightLongitude, topLeftLongitude, topRightLongitude;
    public double bottomLeftLatitude, bottomRightLatitude, topLeftLatitude, topRightLatitude;
    // Distance the user is from the bottom left of map, used to calculate where to show the user indicator on map
    // Given in percent, have to use floats for Vectors
    float mapDistX, mapDistY;
    // Coroutine for location update
    Coroutine updateLocationCO;
    // GPS on/off button image in options menu
    public ButtonImageToggle optionsGPSImg;
    // GPSButtonsScript ref to hide and show buttons during pop up
    GPSButtonsScript gpsBtnScript;

    //Reference the contact points that are downloaded from the locally stored database file.
     ContactPointCollection contactPoints = null;
    // Panel for telling that the player is not in are
    public GameObject notInAreaPanel;
    public bool InMapArea
    {
        get
        {
            return inMapArea;
        }
        set
        {
            inMapArea = value;
        }
    }
    bool inMapArea;
    CameraMovementGPS cameraMovementScript;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        isInitializing = true;
        debugText.text = "Start";
        infoAnim = infoPanelObj.GetComponent<Animator>();
        gpsBtnScript = FindObjectOfType<GPSButtonsScript>();
        canOpenMenu = false;

        // Set map to correct position. First the map is moved to origo (0,0,0), then moved right and up so
        // the bottom left corner of map is in origo, and the map extends right and up.
        Vector3 wantedPos = Vector3.zero;
        RectTransform rt = mapImage.GetComponent<RectTransform>();
        wantedPos.x += rt.rect.width/2;
        wantedPos.y += rt.rect.height/2;     
        rt.position = wantedPos;

        // Set camera start position to center of map
        Vector3 cameraStartPos;
        cameraStartPos = mapImage.transform.position;
        cameraStartPos.z = -10f;
        Camera.main.transform.position = cameraStartPos;

        cameraMovementScript = FindObjectOfType<CameraMovementGPS>();

        // Set correct desiredPosition in camera movement script
        cameraMovementScript.SetStartDesiredPosition(cameraStartPos);

        // Get map image corners's world positions to mapCorners array
        mapImage.GetComponent<RectTransform>().GetWorldCorners(mapCorners);
        
        // Android permissions
        #if UNITY_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        #endif

        #if UNITY_ANDROID && !UNITY_EDITOR
        yield return new WaitForSeconds(5f);

        // Need to tell the user about everything!

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
		{
			debugText.text = "Not Enabled";
            optionsGPSImg.SetImg(false);
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
            optionsGPSImg.SetImg(false);
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            debugText.text = "Unable to determine device location";
            optionsGPSImg.SetImg(false);
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            //debugText.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
			debugText.text = "Working, yay!";
            tracking = true;
            lastDistance = double.MaxValue;
            optionsGPSImg.SetImg(true);
            updateLocationCO = StartCoroutine(UpdateLocation());
        }

        if(!File.Exists(Application.persistentDataPath + "/GPSDatas.txt"))
        {
            File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", String.Empty);
        }

        #elif UNITY_EDITOR
        yield return null;
        debugText.text = "Working, yay!";
        tracking = true;
        optionsGPSImg.SetImg(false);
        lastDistance = double.MaxValue;
        updateLocationCO = StartCoroutine(UpdateLocation());
        #endif

        // Add listeners to location buttons
        for(int i = 0; i < locations.Length; i++)
        {
            int tmp = i;
            locBtns.Add(locations[i].image.transform.GetComponent<Button>());
            locBtns[i].onClick.AddListener(delegate {PushLocBtn(tmp);});
        }
        isInitializing = false;
    }

    // For restarting the gps
    IEnumerator ReStartGPS()
    {
        isInitializing = true;
        // Android permissions
        if(!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        yield return new WaitForSeconds(5f);

        // Need to tell the user about everything!

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
            lastDistance = double.MaxValue;
            updateLocationCO = StartCoroutine(UpdateLocation());
        }
        isInitializing = false;
    }

    // Activate pop-up and set the correct texts
    public void PushLocBtn(int locationIndex)
    {
        Debug.Log(locationIndex);
        if(lastLocation != null)
            Debug.Log("LAST LOCATION: " + lastLocation.name);
        Debug.Log("LOCINDEX LOCATION: " + locations[locationIndex].name);

        cameraMovementScript.CanMove = false;

        //load info from a file
        contactPoints = TriviaSaveLoadSystem.LoadContactPoints();
        ContactPoint currentLocationData = null;
        if (contactPoints == null)
        {
            Debug.LogWarning("You propably run in editor. Contact points are null");
        }
        else {
        for (int i = 0; i < contactPoints.points.Length; i++)
        {
            if( contactPoints.points[i].identifier.ToString() == locations[locationIndex].identifier) {
                currentLocationData = contactPoints.points[i];
                break;
            }
        }
        }
        
        bool contactPointValidation = (contactPoints !=null && locationIndex <= contactPoints.points.Length && currentLocationData !=null);
        string locationNameFromFile = (contactPointValidation) ? currentLocationData.name : locations[locationIndex].name;
        string locationDescriptionFromFile = (contactPointValidation) ? currentLocationData.description : locations[locationIndex].description;

        

        // Open pop-up and show correct info
        infoPanelObj.GetComponent<DisableScript>().enabled = false;
        infoPanelObj.SetActive(true);
        gpsBtnScript.HideOrShowButtons(true);
        infoAnim.SetBool("ShowPanel", true);
        locationTitleText.text = locationNameFromFile;
        locationDescText.text = locationDescriptionFromFile;

        // Set listener for play button
        playBtn.onClick.AddListener(delegate {PushPlayBtn(locBtns[locationIndex].transform.name);});
        triviaBtn.onClick.AddListener(delegate{PushTriviaBtn(locationIndex);});

        if(canOpenMenu && lastLocation != null && lastLocation.name == locations[locationIndex].name)
        {

            // Example classify = (input >= 0) ? "nonnegative" : "negative";
            string localizedLocation = (LocalizationManager.Instance !=null) ? LocalizationManager.Instance.GetLocalizedValue("location_proximity_in") : "Olet alueella" ;
            popupLocText.text = localizedLocation;
            popupLocImg.color = Color.green;
        }
        else
        {
            // Example classify = (input >= 0) ? "nonnegative" : "negative";
            string localizedLocation = (LocalizationManager.Instance !=null) ? LocalizationManager.Instance.GetLocalizedValue("location_proximity_off") : " Et ole alueella" ;
            popupLocText.text = localizedLocation;
            popupLocImg.color = Color.red;
        }
    }

    // Possibly add checks to see if the player can play the game

    // What happens when the player pushes the play button
    void PushPlayBtn(string nameOfGame)
    {
        SceneManager.LoadScene(nameOfGame);
    }

    // What happens when the player pushes the trivia button
    void PushTriviaBtn(int locIndex)
    {
        if(lastLocation.name == locations[locIndex].name && canOpenMenu)
        {
            SetCurrentContactPointForTrivia(locations[locIndex].name);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Trivia",UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            StartCoroutine(ShowNotInAreaPanel());
        }
    }

    IEnumerator ShowNotInAreaPanel()
    {
        notInAreaPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        notInAreaPanel.SetActive(false);
    }

    // Hide the popup panel
    public void DisablePanel()
    {
        playBtn.onClick.RemoveAllListeners();
        triviaBtn.onClick.RemoveAllListeners();
        gpsBtnScript.HideOrShowButtons(false);
        infoAnim.SetBool("ShowPanel", false);
    }

    public void ToggleGPS()
    {
        if(tracking)
        {
            StopCoroutine(updateLocationCO);
            tracking = false;
            Input.location.Stop();
            Debug.Log("NO: " + isInitializing);
            debugText.text = "NO GPS!!!";
        }
        else
        {
            StartCoroutine(ReStartGPS());
        }
    }

    public void StopGPS()
    {
        StopCoroutine(updateLocationCO);
        tracking = false;
        Input.location.Stop();
        Debug.Log("NO: " + isInitializing);
        debugText.text = "NO GPS!!!";
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

    void DebugWriteMapData()
    {
        if(!File.Exists(Application.persistentDataPath + "/MapData.txt"))
        {
            File.AppendAllText(Application.persistentDataPath + "/MapData.txt", String.Empty);
        }

        serializedData = "Longitude: " + longitude.ToString() + "\n"
        + "Latitude: " + latitude.ToString() + "\n"
        + "PercentX: " + mapDistX.ToString() + "\n"
        + "PercentY: " + mapDistY.ToString() + "\n";
        File.AppendAllText(Application.persistentDataPath + "/MapData.txt", serializedData);
    }

    void CheckClosestLoc()
    {
        double tempDist;
        lastDistance = double.MaxValue;
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

    bool IsPlayerInsideMap()
    {
        if(longitude < bottomLeftLongitude || longitude > bottomRightLongitude || latitude > topRightLatitude || latitude < bottomLeftLatitude)
        {
            inMapArea = false;
            return false;
        }
        else
        {
            inMapArea = true;
            return true;
        }
    }

    void UpdatePlayerIndicator()
    {
        double x1 = longitude - topLeftLongitude;
        double x2 = topRightLongitude - longitude;
        double x3 = x1 + x2;
        mapDistX = (float)(x1 / x3);
        double y1 = latitude - bottomRightLatitude;
        double y2 = topRightLatitude - latitude;
        double y3 = y1 + y2;
        mapDistY = (float)(y1 / y3);
        Vector3 endPos;
        endPos.x = mapCorners[2].x * mapDistX;
        endPos.y = mapCorners[2].y * mapDistY;
        endPos.z = 0f;
        userIndicatorImg.transform.position = endPos;
    }

    IEnumerator UpdateLocation()
    {
        // DateTime now = DateTime.Now;
        // File.AppendAllText(Application.persistentDataPath + "/GPSDatas.txt", "-+-+-+-+-" + now.ToString("F") + "-+-+-+-+-"  +"\n");
        // logIndex = 0;
        while(tracking)
        {
            if(!IsPlayerInsideMap())
            {
                // If the player isn't within map, disable GPS
                //StopGPS();
            }
            Debug.Log("UPDATELOC: " + isInitializing);
            #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            longitude = Input.location.lastData.longitude;
			latitude = Input.location.lastData.latitude;
            CheckClosestLoc();
            UpdatePlayerIndicator();  

            // DebugWriteMapData();
            // if(lastDistance < rangeDistance)
            // {
            //     debugText.text = "IN " + lastLocation.name;
            // }
            // else
            // {
            //     debugText.text = "Dist: " + lastDistance;
            // }

            // Debug location
            // if(lastDistance < lastLocation.rangeDistance)
            // {
            //     debugText.text = "IN " + lastLocation.name;
            // }
            // else
            // {
            //     debugText.text = "Dist: " + lastDistance;
            // }

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
            UpdatePlayerIndicator();
            // Debug location
            // if(lastDistance < lastLocation.rangeDistance)
            // {
            //     debugText.text = "IN " + lastLocation.name;
            // }
            // else
            // {
            //     debugText.text = "Dist: " + lastDistance;
            // }
            yield return new WaitForSeconds(1f);

            #endif
        }
    }

    private void SetCurrentContactPointForTrivia(string currentLocationName = null) {
        

        //identified might be modified in the future to be more descriptive. like HAM473, or RaatiH123
        switch (currentLocationName)
        {
            case "Koulu":
            PlayerPrefs.SetInt("TriviaAllRandom", 1);
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

    // Getters and setters
    public Image GetUserIndicatorImg() { return userIndicatorImg; }

    // GPS functions
    // https://answers.unity.com/questions/1221259/how-to-get-distance-from-2-locations-with-unity-lo.html
	double DegToRad(double deg)
	{
		double temp;
		temp = (deg * Math.PI) / 180.0f;
		temp = Math.Tan(temp);
		return temp;
	}
 
  	double Distance_x(double lon_a, double lon_b, double lat_a, double lat_b)
	{
		double temp;
		double c;
		temp = (lat_b - lat_a);
		c = Math.Abs(temp * Math.Cos((lat_a + lat_b)) / 2);
		return c;
	}
 
	private double Distance_y(double lat_a, double lat_b)
	{
		double c;
		c = (lat_b - lat_a);
		return c;
	}
 
	double Final_distance(double x, double y)
	{
		double c;
		c = Math.Abs(Math.Sqrt(Math.Pow(x, 2f) + Math.Pow(y, 2f))) * 6371;
		return c;
	}
 
     //*******************************
 //This is the function to call to calculate the distance between two points
 
	public double Calculate_Distance(double long_a, double lat_a,double long_b, double lat_b )
	{
		double a_long_r, a_lat_r, p_long_r, p_lat_r, dist_x, dist_y, total_dist;
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
