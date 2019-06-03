using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectS : MonoBehaviour {

    private SpriteRenderer SpriR;
    private string Last = "LastNum";
    private string LastShade = "LastShade";
    bool Same = false;

    public int ShaderNum = 0;
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
        if (ShaderNum == PlayerPrefs.GetInt(LastShade))
        {
            Same = true;
        }

        if (ShaderNum == 0)
        {
            int r = Random.Range(0, Right.Length);
            if (Same)
            {
                if (r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if (r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Right.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Right[r];
        }
        else if (ShaderNum == 1)
        {
            int r = Random.Range(0, Left.Length);
            if (Same)
            {
                if (r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if (r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Left.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Left[r];
        }
        else if (ShaderNum == 2)
        {
            int r = Random.Range(0, Up.Length);
            if (Same)
            {
                if (r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if (r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Up.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Up[r];
        }
        else
        {
            int r = Random.Range(0, Down.Length);
            if (Same)
            {
                if (r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if (r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Down.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Down[r];
        }
        PlayerPrefs.SetInt(LastShade, ShaderNum);
    }

    private void Easy()
    {
        Name = GameObject.Find("CountryName").GetComponent<TMP_Text>();

        SpriR = GetComponent<SpriteRenderer>();
        ShaderNum = Random.Range(0, 4);
        if(ShaderNum == PlayerPrefs.GetInt(LastShade))
        {
            Same = true;
        }

        if (ShaderNum == 0)
        {
            int r = Random.Range(0, Right.Length);
            if (Same)
            {
                if(r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if(r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Right.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Right[r];
            Name.text = NRight[r];
        }
        else if (ShaderNum == 1)
        {
            int r = Random.Range(0, Left.Length);
            if (Same)
            {
                if (r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if (r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Left.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Left[r];
            Name.text = NLeft[r];
        }
        else if (ShaderNum == 2)
        {
            int r = Random.Range(0, Up.Length);
            if (Same)
            {
                if (r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if (r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Up.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Up[r];
            Name.text = NUp[r];
        }
        else
        {
            int r = Random.Range(0, Down.Length);
            if (Same)
            {
                if (r == PlayerPrefs.GetInt(Last) && r != 0)
                {
                    r--;
                }
                else if (r == PlayerPrefs.GetInt(Last) && r == 0)
                {
                    r = Down.Length - 1;
                }
            }
            PlayerPrefs.SetInt(Last, r);
            SpriR.sprite = Down[r];
            Name.text = NDown[r];
        }
        PlayerPrefs.SetInt(LastShade, ShaderNum);
    }

}
