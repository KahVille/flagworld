﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    [Tooltip("Drag and drop the earth object here!")]
    public GameObject earth;
    Camera mainCam;
    public float rotSpeed;
    public float minZoom;
    public float maxZoom;
    Vector2 touchStartPos;
    Vector2 touchDirection;
    float touchAngle;
    float timer = 0f;
    public bool CanTouch
    {
        get
        {
            return canTouch;
        }
        set
        {
            canTouch = value;
        }
    }
    bool canTouch = true;

    EarthMapGameManager emGM;

    Quaternion earthStartRot;
    Vector3 cameraStartPos;

    void Start() 
    {
        mainCam = Camera.main;    
        cameraStartPos = mainCam.transform.position;
        earthStartRot = earth.transform.rotation;
        emGM = FindObjectOfType<EarthMapGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchInput();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    public void ResetPoSLoc()
    {
        mainCam.transform.position = cameraStartPos;
        earth.transform.rotation = earthStartRot;
    }

    void TouchInput()
    {
        #if UNITY_EDITOR
        if(Input.GetMouseButton(0))
        {
            if(Input.GetAxis("Mouse X") > 0)
            {
                earth.transform.Rotate(0f, rotSpeed, 0f, Space.World);
            }
            else if(Input.GetAxis("Mouse X") < 0)
            {
                earth.transform.Rotate(0f, -rotSpeed, 0f, Space.World);
            }
            else if(Input.GetAxis("Mouse Y") > 0)
            {
                earth.transform.Rotate(rotSpeed, 0f, 0f, Space.World);
            }
            else if(Input.GetAxis("Mouse Y") < 0)
            {
                earth.transform.Rotate(-rotSpeed, 0f, 0f, Space.World);
            }
        }

        #elif UNITY_IPHONE || UNITY_ANDROID
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                timer = 0f;
            }

            // Raycast to earth to see where it hit
            if(touch.phase == TouchPhase.Ended && timer <= 1f)
            {
                Ray ray = mainCam.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.name == "AsiaCol")
                    {
                        emGM.StartMapTransition(true);
                    }
                }
            }

            touchDirection = touch.position - touchStartPos;
            earth.transform.Rotate(touchDirection.y * rotSpeed, -touchDirection.x * rotSpeed, 0f, Space.World);
            
            timer += Time.deltaTime;
        }
        else if(Input.touchCount == 2)
        {
            // https://unity3d.com/learn/tutorials/topics/mobile-touch/pinch-zoom
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if(deltaMagnitudeDiff > 0f && mainCam.transform.localPosition.z > maxZoom)
            {

            }
            else if(deltaMagnitudeDiff < 0f && mainCam.transform.localPosition.z < minZoom)
            {

            }
            else
            {
                mainCam.transform.Translate(Vector3.forward * Time.deltaTime * deltaMagnitudeDiff);
            }   
        }
        
        #endif
    }
}
