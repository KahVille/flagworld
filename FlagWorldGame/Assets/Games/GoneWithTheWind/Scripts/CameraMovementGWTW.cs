using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementGWTW : MonoBehaviour
{
    public Transform target;

    // Speed of scrolling
    public float smoothSpeed;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;
    }
}
