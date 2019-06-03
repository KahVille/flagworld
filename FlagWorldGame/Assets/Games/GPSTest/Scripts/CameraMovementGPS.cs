// In charge of camera movement in GPS map scene.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraMovementGPS : MonoBehaviour
{
    Camera mainCam;
    Vector3 touchStartPos;                  // Position where the touch started
    Vector3 touchDirection;                 // Direction vector from touch start position to current touch position
    public float speed;                     // Speed value used in smoothdamp
    public float scrollSpeed;               // Use this to modify the speed of scrolling
    public float swipeValue;                // Adjust to make the camera move faster or slower 
    float zoomDivisor = 7000f;              // Value used in zoom stuff to make values better
    public float zoomSpeed;                 // Adjust zoom speed, higher value means slower zoom
    Vector3 desiredPosition;                // The position camera tries to get to
    Vector3 velocity;                       // For the Smoothdamp function
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
    public bool CanMove
    {
        get
        {
            return canMove;
        }
        set
        {
            canMove = value;
        }
    }
    Swipe swipe;                            // Swipe script for swipe stuff
    SuperSwipe ss;
    public TextMeshProUGUI debugText;


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        mainCam = Camera.main;    
        initialOrtoSize = mainCam.orthographicSize;
        gpsScript = FindObjectOfType<GPSScript>();
        imgBounds = gpsScript.mapImage.GetComponent<Image>();
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        TouchInput();
        #elif UNITY_EDITOR
        DebugInput();
        #endif
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, speed);
        RecalculateBounds();
        ClampCameraPos();
    }

    // Function for setting the beginning desired position. This is called in GPS script start to get the right start position.
    // Need to do this because of the order the scripts are loaded.
    public void SetStartDesiredPosition(Vector3 newPos)
    {
        desiredPosition = newPos;
    }

    // Used to test camera move and clamping on the editor
    void DebugInput()
    {
        if(!canMove)
        {
            return;
        }
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        zoomMultiplier += mouseScroll;
        zoomMultiplier = Mathf.Clamp(zoomMultiplier, minZoomMult, maxZoomMult);
        //debugText.text = zoomMultiplier.ToString();
        mainCam.orthographicSize = initialOrtoSize * zoomMultiplier;

        float xMove = Input.GetAxis("Horizontal") * 0.5f;
        float yMove = Input.GetAxis("Vertical") * 0.5f;
        transform.Translate(xMove, yMove, 0f);
        //Vector3 temp = swipe.SwipeDelta;
        // Adjust the value to make the camera move slower or quicker

    }

    void TouchInput()
    {
        if(!canMove)
        {
            return;
        }
       
        
        // Zoom with 2 fingers
        if(Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            firstTouchVec = touch2.position - touch1.position;      // Vector between first touch points

            // Get the magnitude of vector that is formed in the points of 2 touch inputs
            if((touch1.phase == TouchPhase.Began) || touch2.phase == TouchPhase.Began)
            {
                firstMagnitude = firstTouchVec.magnitude;
                firstMagnitude /= zoomDivisor;
            }

            curMagnitude = firstTouchVec.magnitude - firstMagnitude;
            // Make value more sensible
            curMagnitude /= zoomDivisor;
            // Check whether to zoom in or out
            if(firstMagnitude > (firstTouchVec.magnitude / zoomDivisor))
            {
                zoomMultiplier += curMagnitude / zoomSpeed;
            }
            else
            {
                zoomMultiplier -= curMagnitude / zoomSpeed;   
            }
            zoomMultiplier = Mathf.Clamp(zoomMultiplier, minZoomMult, maxZoomMult);
            //debugText.text = firstMagnitude.ToString() + "|" + (firstTouchVec).ToString();

            mainCam.orthographicSize = initialOrtoSize * zoomMultiplier;

            if(touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                StartCoroutine(CooldownCoroutine());
            }
        }

        // Move the map with 1 finger
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchStartPos = mainCam.ScreenToWorldPoint(touch.position);
                touchTimer = 0;
            }

            if(touch.phase == TouchPhase.Ended && touchTimer < 0.1f)
            {
                Vector3 newCamPos = gpsScript.GetUserIndicatorImg().transform.position;
                newCamPos.z = -10f;
                mainCam.transform.position = newCamPos;
                return;
            }

            touchDirection = touchStartPos - mainCam.ScreenToWorldPoint(touch.position);
            desiredPosition += touchDirection * scrollSpeed;
            desiredPosition.z = -10f;

            // if(touchDirection.magnitude > 70f)
            // {
            //     desiredPosition.x += touchDirection.x;
            //     desiredPosition.y += touchDirection.y;
            //     desiredPosition /= 20f;
            //     desiredPosition.z = -10f;
            // }

            // debugText.text = touchDirection.magnitude.ToString();
            //mainCam.transform.Translate(touchDirection.x * (speed*0.1f) * Time.deltaTime, touchDirection.y * (speed*0.1f) * Time.deltaTime, 0f);

            touchTimer += Time.deltaTime;
            
        }
        
         // Test input for moving the camera, so the user can scroll the map
        #region TestPanning
        // Input for moving the camera, so the user can scroll the map
        // if(Input.touchCount == 1)
        // {
        //     /*
        //     Vector3 temp = swipe.SwipeDelta;
        //     // Adjust the value to make the camera move slower or quicker
        //     temp *= swipeValue;
        //     desiredPosition -= temp;
        //     desiredPosition.z = -10f;
        //     */
        //     // float x = ss.HorizontalMoveDistance();
        //     // float y = ss.VerticalMoveDistance();
        //     float x = ss.DistVec().x;
        //     float y = ss.DistVec().y;
        //     debugText.text = x.ToString() + "|" + y.ToString();
        //     desiredPosition = new Vector3(x, y, -10f);
        // }
        #endregion

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
        desiredPosition = clampedPos;
    }

    IEnumerator CooldownCoroutine()
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }

    public void SetCanMove(bool newCanMove)
    {
        canMove = newCanMove;
    }
}
