using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBG : MonoBehaviour
{
    public Sprite[] options;

    private SpriteRenderer Render;
    private Sprite Psprite;
    private ConnectTile parent;
    private ParticleSystem Pop;

    // Use this for initialization
    void Start()
    {
        Render = GetComponent<SpriteRenderer>();
        parent = GetComponentInParent<ConnectTile>();
        Pop = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Psprite = parent.GetComponent<SpriteRenderer>().sprite;
        if (Psprite == options[0])
        {
            Render.color = new Color32(255, 208, 161, 255);
        }
        else if (Psprite == options[1])
        {
            Render.color = new Color32(253, 161, 240, 255);
        }
        else if (Psprite == options[2])
        {
            Render.color = new Color32(161, 187, 253, 255);
        }
        else if (Psprite == options[3])
        {
            Render.color = new Color32(251, 253, 161, 255);
        }
        else if (Psprite == options[4])
        {
            Render.color = new Color32(253, 170, 161, 255);
        }
        else if (Psprite == options[5])
        {
            Render.color = new Color32(161, 244, 253, 255);
        }
        else if (Psprite == options[6])
        {
            Render.color = new Color32(180, 253, 161, 255);
        }
        ParticleSystem.MainModule main = Pop.main;
        main.startColor = Render.color;
    }
}
