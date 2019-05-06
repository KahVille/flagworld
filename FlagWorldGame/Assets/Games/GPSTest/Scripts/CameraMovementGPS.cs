using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementGPS : MonoBehaviour
{
    Camera mainCam;
    Vector2 touchStartPos;                  // Position where the touch started
    Vector2 touchDirection;                 // Direction vector from touch start position to current touch position
    public float speed;                     // Speed of scrolling 
    public float minX, maxX, minY, maxY;    // Variables for restricting camera movement

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
        TouchInput();
    }

    void TouchInput()
    {
        // Input for moving the camera, so the user can scroll the map
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }

            touchDirection = touch.position - touchStartPos;
            mainCam.transform.Translate(touchDirection.x * (speed*0.1f) * Time.deltaTime, touchDirection.y * (speed*0.1f) * Time.deltaTime, 0f);

            // Restrict camera movement so skybox doesn't show, and we don't go over the map
            Vector3 camPos = mainCam.transform.position;
            camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
            camPos.y = Mathf.Clamp(camPos.y, minY, maxY);
            mainCam.transform.position = camPos;
        }
    }
}
