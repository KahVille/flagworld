using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuistiTile : MonoBehaviour {

    private static MuistiTile previousSelected = null;
    private static bool Started = false;
    private static bool CanClick = true;

    private SpriteRenderer render, maretarium;
    private Sprite CardBack, Picture;
    public Sprite Pair;
    private bool isSelected = false;
    private Color32 back, front;
    private MuistiManager MM;
    private AudioSource AudioS;
    public AudioClip Found;

    [HideInInspector] public bool Paired = false;

    void Start () {
        MM = GameObject.Find("Manager").GetComponent<MuistiManager>();
        render = GetComponent<SpriteRenderer>();
        AudioS = GetComponent<AudioSource>();
        back = render.color;
        front = new Color32(255, 255, 255, 255);
        CardBack = render.sprite;
	}

    private void Select()
    {
        isSelected = true;
        //render.sprite = Picture;
        StartCoroutine(Flip(Picture));
        render.color = front;
        previousSelected = gameObject.GetComponent<MuistiTile>();
    }

    private void Deselect()
    {
        isSelected = false;
        //render.sprite = CardBack;
        StartCoroutine(Flip(CardBack));
        render.color = back;
        previousSelected = null;
    }

    private void OnMouseDown()
    {
        if (Paired || !Started || !CanClick)
        {
            return;
        }

        if (isSelected)
        { // 2 Is it already selected?
            Deselect();
            MM.GetMove();
        }
        else
        {
            if (previousSelected == null)
            { // 3 Is it the first tile selected?
                Select();
            }
            else
            {
                //render.sprite = Picture;
                StartCoroutine(Flip(Picture));
                render.color = front;
                CheckPair();
            }
        }
    }

    private void CheckPair()
    {
        if(Pair == previousSelected.GetPicture())
        {
            Paired = true;
            previousSelected.Paired = true;
            AudioS.clip = Found;
            AudioS.Play();
            previousSelected = null;
        }
        else
        {
            StartCoroutine(WrongPair());
        }
        MM.GetMove();
    }

    private IEnumerator WrongPair()
    {
        CanClick = false;
        yield return new WaitForSeconds(0.6f);
        previousSelected.Deselect();
        Deselect();
        CanClick = true;
    }

    public void SetPicture(Sprite img)
    {
        Picture = img;
    }

    public void SetPair(Sprite img)
    {
        Pair = img;
    }

    public Sprite GetPicture()
    {
        return Picture;
    }

    public void StartPressed()
    {
        if (!Started)
        {
            Started = true;
        }
        else
        {
            Started = false;
        }
    }

    private IEnumerator Flip(Sprite s)
    {
        AudioS.Play();
        for (int i = 0; i < 3; i++)
        {
            transform.Rotate(0, 25, 0);
            yield return new WaitForSeconds(0.05f);
        }
        render.sprite = s;
        for (int i = 0; i < 2; i++)
        {
            transform.Rotate(0, -25, 0);
            yield return new WaitForSeconds(0.05f);
        }
        transform.Rotate(0, -25, 0);
    }
}
