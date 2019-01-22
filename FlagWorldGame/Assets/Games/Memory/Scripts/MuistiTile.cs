using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuistiTile : MonoBehaviour {

    private static MuistiTile previousSelected = null;
    private static bool Started = false;
    private static bool CanClick = true;

    private SpriteRenderer render, maretarium;
    private Sprite CardBack, Picture;
    private bool isSelected = false;
    private Color32 back, front;
    private MuistiManager MM;

    [HideInInspector] public bool Paired = false;

    void Start () {
        MM = GameObject.Find("Manager").GetComponent<MuistiManager>();
        render = GetComponent<SpriteRenderer>();
        //SpriteRenderer[] Srs = gameObject.GetComponentsInChildren<SpriteRenderer>();
        //maretarium = Srs[1];
        back = render.color;
        front = new Color32(255, 255, 255, 255);
        CardBack = render.sprite;
	}

    private void Select()
    {
        isSelected = true;
        render.sprite = Picture;
        render.color = front;
        //maretarium.enabled = false;
        previousSelected = gameObject.GetComponent<MuistiTile>();
    }

    private void Deselect()
    {
        isSelected = false;
        render.sprite = CardBack;
        render.color = back;
        //maretarium.enabled = true;
        previousSelected = null;
    }

    private void OnMouseDown()
    {
        if (Paired || !Started || !CanClick)
        {
            Debug.Log("Not Clickable");
            return;
        }

        Debug.Log("Clickable");
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
                render.sprite = Picture;
                render.color = front;
                //maretarium.enabled = false;
                CheckPair();
            }
        }
    }

    private void CheckPair()
    {
        if(Picture == previousSelected.GetPicture())
        {
            Paired = true;
            previousSelected.Paired = true;
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
        yield return new WaitForSeconds(0.3f);
        previousSelected.Deselect();
        Deselect();
        CanClick = true;
    }

    public void SetPicture(Sprite img)
    {
        Picture = img;
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
}
