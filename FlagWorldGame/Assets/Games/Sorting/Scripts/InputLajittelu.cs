using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLajittelu : MonoBehaviour {

    private GameObject SO;
    private Swipe SW;
    public float Speed = 25;

	// Use this for initialization
	void Start () {
        SW = GetComponent<Swipe>();
	}
	
	// Update is called once per frame
	void Update () {
        if (SW.SwipeLeft)
        {
            SO.GetComponent<Rigidbody2D>().velocity = new Vector2(-Speed, 0);
        }
        else if (SW.SwipeRight)
        {
            SO.GetComponent<Rigidbody2D>().velocity = new Vector2(Speed, 0);
        }
        else if (SW.SwipeUp)
        {
            SO.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Speed + 5);
        }
        else if (SW.SwipeDown)
        {
            SO.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Speed - 5);
        }
	}

    public void SetSortable(GameObject s)
    {
        SO = s;
    }
}
