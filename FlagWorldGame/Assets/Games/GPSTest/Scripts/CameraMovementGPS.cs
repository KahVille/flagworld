﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraMovementGPS : MonoBehaviour
{
    Camera mainCam;
    Vector2 touchStartPos;                  // Position where the touch started
    Vector2 touchDirection;                 // Direction vector from touch start position to current touch position
    public float speed;                     // Speed of scrolling 
    float minX, maxX, minY, maxY;           // Variables for restricting camera movement
    float horizontalExt, verticalExt;       // Camera movement restriction
    public float zoomMultiplier;            // Affects camera zoom and camera movement restrictions
    public float minZoomMult, maxZoomMult;  // Variables for restricting zoom
    Image imgBounds;                        // Used for the bounds of map
    float initialOrtoSize;                  // The initial camera ortographic size
    float touchTimer;                       // Used to check how long the touch is
    GPSScript gpsScript;                    // Needed to center the camera on player
    float firstMagnitude;                   // Magnitude of vector when 2 fingers first touched the screen
    Vector2 firstTouchVec;                  // Vector between two finger touches
    float curMagnitude;                     // Updated magnitude 
    bool canMove;                           // Used to check if can move
    public TextMeshProUGUI debugText;


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        mainCam = Camera.main;    
        initialOrtoSize = mainCam.orthographicSize;
        gpsScript = FindObjectOfType<GPSScript>();
        imgBounds = gpsScript.mapImage.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        TouchInput();
        #elif UNITY_EDITOR
        DebugInput();
        #endif
    }

    // Used to test camera move and clamping on the editor
    void DebugInput()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        zoomMultiplier += mouseScroll;
        zoomMultiplier = Mathf.Clamp(zoomMultiplier, minZoomMult, maxZoomMult);
        debugText.text = zoomMultiplier.ToString();
        mainCam.orthographicSize = initialOrtoSize * zoomMultiplier;

        float xMove = Input.GetAxis("Horizontal") * 0.5f;
        float yMove = Input.GetAxis("Vertical") * 0.5f;
        transform.Translate(xMove, yMove, 0f);
        RecalculateBounds();
        ClampCameraPos();
    }

    void TouchInput()
    {
        // Input for moving the camera, so the user can scroll the map
        if(Input.touchCount == 1)
        {
            if(!canMove)
            {
                return;
            }

            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchTimer = 0;
            }

            if(touch.phase == TouchPhase.Ended && touchTimer < 0.1f)
            {
                Vector3 newCamPos = gpsScript.GetUserIndicatorImg().transform.position;
                newCamPos.z = -10f;
                mainCam.transform.position = newCamPos;
                RecalculateBounds();
                ClampCameraPos();
                return;
            }

            touchDirection = touch.position - touchStartPos;
            mainCam.transform.Translate(touchDirection.x * (speed*0.1f) * Time.deltaTime, touchDirection.y * (speed*0.1f) * Time.deltaTime, 0f);
            RecalculateBounds();
            ClampCameraPos();

            touchTimer += Time.deltaTime;
        }
        // Zoom with 2 fingers
        else if(Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            canMove = false;
            firstTouchVec = touch2.position - touch1.position;      // Vector between first touch points

            // Get the magnitude of vector that is formed in the points of 2 touch inputs
            if(touch1.phase == TouchPhase.Began)
            {
                firstMagnitude = firstTouchVec.magnitude;
            }

            curMagnitude = firstTouchVec.magnitude;
            // Make value more sensible
            curMagnitude /= 700f;
            zoomMultiplier = curMagnitude;
            zoomMultiplier = Mathf.Clamp(zoomMultiplier, minZoomMult, maxZoomMult);
            debugText.text = firstMagnitude.ToString() + "|" + curMagnitude.ToString();

            mainCam.orthographicSize = initialOrtoSize * zoomMultiplier;
            RecalculateBounds();
            ClampCameraPos();

            if(touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                StartCoroutine(CooldownCoroutine());
            }
        }
    }

    // https://answers.unity.com/questions/682751/keep-top-down-2d-camera-in-bounds-of-background-sp.html
    void RecalculateBounds()
    {
        float height = 2f * mainCam.orthographicSize;
        float width = height * mainCam.aspect;
        //Vector2 bottomLeft = mainCam.ViewportToWorldPoint(new Vector2(0, 0));  
        //Vector2 topRight = mainCam.ViewportToWorldPoint(new Vector2(1, 1));  
        //horizontalExt = verticalExt * Screen.width / Screen.height;
        minX = gpsScript.mapCorners[0].x + (width/2);
        maxX = gpsScript.mapCorners[2].x - (width/2);
        minY = gpsScript.mapCorners[0].y + (height/2);
        maxY = gpsScript.mapCorners[2].y - (height/2);
        // minX = (float)(horizontalExt - gpsScript.mapCorners[0].x);
        // maxX = (float)(gpsScript.mapCorners[1].x - horizontalExt);
        // minY = (float)(verticalExt - gpsScript.mapCorners[0].y);
        // maxX = (float)(gpsScript.mapCorners[2].y - verticalExt);
    }

    // Used to clamp the camera position
    void ClampCameraPos()
    {
        Vector3 clampedPos = mainCam.transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        mainCam.transform.position = clampedPos;
    }

    IEnumerator CooldownCoroutine()
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
