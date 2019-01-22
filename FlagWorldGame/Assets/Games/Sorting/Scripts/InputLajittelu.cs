using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLajittelu : MonoBehaviour {

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
            GameObject.FindGameObjectWithTag("Sortable").GetComponent<Rigidbody2D>().velocity = new Vector2(-Speed, 0);
        }
        else if (SW.SwipeRight)
        {
            GameObject.FindGameObjectWithTag("Sortable").GetComponent<Rigidbody2D>().velocity = new Vector2(Speed, 0);
        }
        else if (SW.SwipeUp)
        {
            GameObject.FindGameObjectWithTag("Sortable").GetComponent<Rigidbody2D>().velocity = new Vector2(0, Speed + 5);
        }
        else if (SW.SwipeDown)
        {
            GameObject.FindGameObjectWithTag("Sortable").GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Speed - 5);
        }
	}
}
