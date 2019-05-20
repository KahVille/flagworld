using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProjectionMatrix : MonoBehaviour
{
    float orthographicSize;
    public float aspect = 1.77777f;

    void Start()
    {
        orthographicSize = Camera.main.orthographicSize;
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                GetComponent<Camera>().nearClipPlane, GetComponent<Camera>().farClipPlane);
    }

}
