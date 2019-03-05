using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuffGWTW : MonoBehaviour
{
    Swipe swipe;
    public GameObject flag;
    public Transform topOfPole;
    public Transform bottomOfPole;
    public float smoothTime = 0.3f;
    Vector3 desiredPos;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        swipe = GetComponent<Swipe>();
        desiredPos = flag.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit(); 
        }
    
        desiredPos.y -= (swipe.SwipeDelta.y * 0.01f);
        desiredPos.y = Mathf.Clamp(desiredPos.y, bottomOfPole.position.y, topOfPole.position.y);
        flag.transform.position = Vector3.SmoothDamp(flag.transform.position, desiredPos, ref velocity, smoothTime);
    }
}
