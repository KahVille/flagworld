using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthMapGameManager : MonoBehaviour
{
    TransitionScript tsScript;
    RotatePlanet rsScript;
    public MapInput mapInputScript;
    Camera mainCam;
    public GameObject cameraEndPosObj;
    public GameObject earth;

    public GameObject asiaMap;
    public GameObject backPlanetBtn;
    public GameObject backMapBtn;
    public GameObject mapStuff;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        tsScript = FindObjectOfType<TransitionScript>();
        rsScript = FindObjectOfType<RotatePlanet>();
        StartCoroutine(tsScript.FadeImg());
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void BackFromPlanet()
    {
        SceneManager.LoadScene(0);
    }

    public void StartMapTransition(bool toMap)
    {
        if(!tsScript.IsTransitioning)
        {
            StartCoroutine(MapViewTransition(toMap));
        }
    }

    public IEnumerator MapViewTransition(bool toMap)
    {
        if(toMap)
        {
            StartCoroutine(tsScript.FadeImg(3f, false));
            float lerp = 0f;
            Vector3 startPos = mainCam.transform.position;
            Vector3 endPos = cameraEndPosObj.transform.position;
            rsScript.enabled = false;
            backPlanetBtn.SetActive(false);
            backMapBtn.SetActive(true);
            while(lerp <= 1f)
            {
                mainCam.transform.position = Vector3.Lerp(startPos, endPos, lerp);
                lerp += Time.deltaTime / 1.5f;
                yield return null;
            }
            while(tsScript.IsTransitioning)
            {
                yield return null;
            }
            asiaMap.SetActive(true);
            mapStuff.SetActive(true);
            mapInputScript.enabled = true;
            StartCoroutine(tsScript.FadeImg(3f, true));
        }
        else
        {
            StartCoroutine(tsScript.FadeImg(3f, false));
            rsScript.enabled = true;
            rsScript.ResetPoSLoc();
            backPlanetBtn.SetActive(true);
            backMapBtn.SetActive(false);
            while(tsScript.IsTransitioning)
            {
                yield return null;
            }
            asiaMap.SetActive(false);
            mapStuff.SetActive(false);
            mapInputScript.enabled = false;
            StartCoroutine(tsScript.FadeImg(3f, true));
        }
    }
}
