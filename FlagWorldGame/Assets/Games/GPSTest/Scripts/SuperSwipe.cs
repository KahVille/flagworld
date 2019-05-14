// Handles the swipe on GPS map
// https://www.youtube.com/watch?time_continue=2&v=jbFYYbu5bdc

using UnityEngine;

// Tells the direction of the swipe
public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class SuperSwipe : MonoBehaviour
{
    // Position the finger is currently touching
    private Vector2 currentPos;
    // Last position of finger
    private Vector2 endPos;

    [SerializeField]
    private float minDistanceForSwipe = 20f;


    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                currentPos = touch.position;
                endPos = touch.position;
            }

            if(touch.phase == TouchPhase.Moved)
            {
                currentPos = touch.position;
                DetectSwipe();
            }
            
            if(touch.phase == TouchPhase.Ended)
            {
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {
        if(SwipeDistanceCheckMet())
        {
            if(IsVerticalSwipe())
            {
                SwipeDirection direction = currentPos.y - endPos.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
            }
            else
            {
                SwipeDirection direction = currentPos.x - endPos.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
            }
            endPos = currentPos;
        }
    }

    bool IsVerticalSwipe()
    {
        return VerticalMoveDistance() > HorizontalMoveDistance();
    }

    bool SwipeDistanceCheckMet()
    {
        return VerticalMoveDistance() > minDistanceForSwipe 
        || HorizontalMoveDistance() > minDistanceForSwipe;
    }

    public float VerticalMoveDistance()
    {
        return Mathf.Abs(currentPos.y - endPos.y);
    }

    public float HorizontalMoveDistance()
    {
        return Mathf.Abs(currentPos.x - endPos.x);
    }
}
