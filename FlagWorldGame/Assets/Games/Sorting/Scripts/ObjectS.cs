using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectS : MonoBehaviour {

    private SpriteRenderer SpriR;

    public int ShaderNum;
    public Sprite[] Right;
    public Sprite[] Left;
    public Sprite[] Up;
    public Sprite[] Down;

    private void Awake()
    {
        SpriR = GetComponent<SpriteRenderer>();
        ShaderNum = Random.Range(0, 4);

        if (ShaderNum == 0) {
            int r = Random.Range(0, Right.Length);
            SpriR.sprite = Right[r];
        }
        else if(ShaderNum == 1)
        {
            int r = Random.Range(0, Left.Length);
            SpriR.sprite = Left[r];
        }
        else if(ShaderNum == 2)
        {
            int r = Random.Range(0, Up.Length);
            SpriR.sprite = Up[r];
        }
        else
        {
            int r = Random.Range(0, Down.Length);
            SpriR.sprite = Down[r];
        }
    }

    //private void Update()
    //{
       
    //}
}
