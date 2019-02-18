using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProgressCircle : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectComponent = null;
    private float rotateSpeed = 200f;

    private void OnDisable() {
      rectComponent.Rotate(0f, 0f, 0f);  
    }

    private void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
