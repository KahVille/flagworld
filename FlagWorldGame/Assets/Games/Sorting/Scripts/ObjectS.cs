using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectS : MonoBehaviour {

    private SpriteRenderer SpriR;

    public int ShaderNum;
    public Sprite[] Right;
    public Sprite[] Left;
    public Sprite[] Up;
    public Sprite[] Down;

    //Name system for easy
    private TMP_Text Name;
    public string[] NRight;
    public string[] NLeft;
    public string[] NUp;
    public string[] NDown;

    private void Awake()
    {

        if (GameObject.Find("Spawner").GetComponent<LajitteluSpawner>().GetHard())
        {
            Hard();
        }
        else
        {
            Easy();
        }
    }

    private void Hard()
    {
        SpriR = GetComponent<SpriteRenderer>();
        ShaderNum = Random.Range(0, 4);

        if (ShaderNum == 0)
        {
            int r = Random.Range(0, Right.Length);
            SpriR.sprite = Right[r];
        }
        else if (ShaderNum == 1)
        {
            int r = Random.Range(0, Left.Length);
            SpriR.sprite = Left[r];
        }
        else if (ShaderNum == 2)
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

    private void Easy()
    {
        Name = GameObject.Find("CountryName").GetComponent<TMP_Text>();

        SpriR = GetComponent<SpriteRenderer>();
        ShaderNum = Random.Range(0, 4);

        if (ShaderNum == 0)
        {
            int r = Random.Range(0, Right.Length);
            SpriR.sprite = Right[r];
            Name.text = NRight[r];
        }
        else if (ShaderNum == 1)
        {
            int r = Random.Range(0, Left.Length);
            SpriR.sprite = Left[r];
            Name.text = NLeft[r];
        }
        else if (ShaderNum == 2)
        {
            int r = Random.Range(0, Up.Length);
            SpriR.sprite = Up[r];
            Name.text = NUp[r];
        }
        else
        {
            int r = Random.Range(0, Down.Length);
            SpriR.sprite = Down[r];
            Name.text = NDown[r];
        }
    }

}
