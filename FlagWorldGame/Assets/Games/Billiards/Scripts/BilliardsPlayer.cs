using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilliardsPlayer : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 DragStart = Vector2.zero;
    private Vector2 DragEnd = Vector2.zero;

    public float forceM = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        DragStart = new Vector2(transform.position.x, transform.position.y);
    }

    private void OnMouseUp()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DragEnd = new Vector2(mouse.x, mouse.y);
        AddForce();
    }

    private void AddForce()
    {
        Vector2 dir = (DragStart - DragEnd);
        rb2d.AddForce(dir*forceM);
    }
}
