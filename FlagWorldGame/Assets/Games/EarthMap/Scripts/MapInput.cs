using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInput : MonoBehaviour
{
    Camera mainCam;
    GameObject currentFlag;
    Vector3 currentTouchPos;
    FlagSpawner fsScript;

    void Start() 
    {
        mainCam = Camera.main;  
        fsScript = FindObjectOfType<FlagSpawner>();  
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<EarthMapGameManager>().StartMapTransition(false);
        }

        currentFlag.transform.position = currentTouchPos;        
    }
    
    void FixedUpdate() 
    {
        MapTouchInput();
    }

    void MapTouchInput()
    {
        #if UNITY_EDITOR
        if(Input.GetMouseButton(0))
        {
            
        }

        #elif UNITY_IPHONE || UNITY_ANDROID
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 tempPosition = touch.position;
            tempPosition.z = 10f;
            Vector3[] cornersOfFlag = new Vector3[4];
            fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().GetWorldCorners(cornersOfFlag);
            
            // Debug.Log("TempPos: " + tempPosition);
            // Debug.Log("rect XMin:" + fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.xMin);
            // Debug.Log("rect XMax:" + fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.xMax);
            // Debug.Log("rect YMin:" + fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.yMin);
            // Debug.Log("rect yMax:" + fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.yMax);
            // Debug.Log("corner0: " + cornersOfFlag[0]);
            // Debug.Log("corner1: " + cornersOfFlag[1]);
            // Debug.Log("corner2: " + cornersOfFlag[2]);
            // Debug.Log("corner3: " + cornersOfFlag[3]);

            // if(tempPosition.x > fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.xMin 
            // && tempPosition.x < fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.xMax 
            // && tempPosition.y > fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.yMin
            // && tempPosition.y < fsScript.GetCurrentFlag().transform.GetComponent<RectTransform>().rect.yMax
            // && currentFlag == null)
            
            if(tempPosition.x > cornersOfFlag[0].x && tempPosition.x < cornersOfFlag[3].x
            && tempPosition.y > cornersOfFlag[0].y && tempPosition.y < cornersOfFlag[1].y
            && currentFlag == null)
            {
                Debug.Log("Flag hit!");
                currentFlag = fsScript.GetCurrentFlag();
                fsScript.SpawnFlag();
            }

            //currentTouchPos = mainCam.ScreenToWorldPoint(tempPosition);
            currentTouchPos = tempPosition;

            // DEBUGGII
            // Debug.Log("CurrentTouchPos: " + currentTouchPos);
            // Debug.Log("FlagPos: " + currentFlag.transform.position);

            // Ray ray = mainCam.ScreenPointToRay(touch.position);
            // RaycastHit hit;
            // if(Physics.Raycast(ray, out hit))
            // {
            //     if(hit.transform.CompareTag("Flag") && currentFlag == null)
            //     {
            //         Debug.Log("Flag hit!");
            //         currentFlag = hit.transform.gameObject;
            //         fsScript.SpawnFlag();
            //     }
            // }
            
            if(touch.phase == TouchPhase.Ended)
            {
                currentFlag = null;
            }
        }
        
        #endif    
    }
}
