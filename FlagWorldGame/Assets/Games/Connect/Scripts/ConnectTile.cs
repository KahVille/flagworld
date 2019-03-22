using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectTile : MonoBehaviour
{
    private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    private static ConnectTile previousSelected = null;
    private ConnectTile temp;

    private SpriteRenderer render;
    private TileSwipe TileSW;
    private bool isSelected = false;

    private Vector2[] adjacentDirections;
    private bool matchFound = false;

    private Sprite previousImage, swappedImage;
    [HideInInspector] public bool swapped = false;
    [HideInInspector] public IEnumerator coroutine = null;
    [HideInInspector] public bool noBack = false;
    [HideInInspector] public bool Started = false;

    private ConnectHUD HUD;
    [HideInInspector] public ParticleSystem Pop;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        HUD = GameObject.Find("HUD").GetComponent<ConnectHUD>();
        Pop = GetComponentInChildren<ParticleSystem>();
        TileSW = GetComponent<TileSwipe>();
    }

    public void SwipeData(string dir)
    {
        if (dir == "Left")
        {
            GameObject T = GetAdjacent(Vector2.left);
            if (T != null)
            {
                ConnectTile TT = T.GetComponent<ConnectTile>();
                SwapSprite(TT.render); // 2
                TT.ClearAllMatches(false, gameObject);
                temp = TT;
                TT.Deselect();
                ClearAllMatches(true, gameObject);
                Deselect();
            }
        }
        else if (dir == "Right")
        {
            GameObject T = GetAdjacent(Vector2.right);
            if (T != null)
            {
                ConnectTile TT = T.GetComponent<ConnectTile>();
                SwapSprite(TT.render); // 2
                TT.ClearAllMatches(false, gameObject);
                temp = TT;
                TT.Deselect();
                ClearAllMatches(true, gameObject);
                Deselect();
            }
        }
        else if (dir == "Up")
        {
            GameObject T = GetAdjacent(Vector2.up);
            if (T != null)
            {
                ConnectTile TT = T.GetComponent<ConnectTile>();
                SwapSprite(TT.render); // 2
                TT.ClearAllMatches(false, gameObject);
                temp = TT;
                TT.Deselect();
                ClearAllMatches(true, gameObject);
                Deselect();
            }
        }
        else if (dir == "Down")
        {
            GameObject T = GetAdjacent(Vector2.down);
            if (T != null)
            {
                ConnectTile TT = T.GetComponent<ConnectTile>();
                SwapSprite(TT.render); // 2
                TT.ClearAllMatches(false, gameObject);
                temp = TT;
                TT.Deselect();
                ClearAllMatches(true, gameObject);
                Deselect();
            }
        }
    }

    private void Select()
    {
        isSelected = true;
        render.color = selectedColor;
        TileSW.SwipeOn = true;
        previousSelected = gameObject.GetComponent<ConnectTile>();
    }

    private void Deselect()
    {
        isSelected = false;
        TileSW.SwipeOn = false;
        render.color = Color.white;
        previousSelected = null;
    }

    void OnMouseDown()
    {
        // 1
        if (render.sprite == null || BoardManager.instance.IsShifting || !Started)
        {
            return;
        }

        if (isSelected)
        { // 2 Is it already selected?
            Deselect();
        }
        else
        {
            if (previousSelected == null)
            { // 3 Is it the first tile selected?
                Select();
            }
            else
            {
                if (GetAllAdjacentTiles().Contains(previousSelected.gameObject))
                { // 1
                    SwapSprite(previousSelected.render); // 2
                    previousSelected.ClearAllMatches(false, gameObject);
                    temp = previousSelected;
                    previousSelected.Deselect();
                    ClearAllMatches(true, gameObject);
                }
                else
                { // 3
                    previousSelected.GetComponent<ConnectTile>().Deselect();
                    Select();
                }
            }
        }
    }

    public void SwapSprite(SpriteRenderer render2)
    { // 1
        if (render.sprite == render2.sprite)
        { // 2
            return;
        }
        previousImage = render.sprite;
        swappedImage = render2.sprite;

        Sprite tempSprite = render2.sprite; // 3
        render2.sprite = render.sprite; // 4
        render.sprite = tempSprite; // 5
        swapped = true;
    }

    private GameObject GetAdjacent(Vector2 castDir) // if Physics2D QueriesStartInCollider is true, gets itself
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
        }

        return adjacentTiles;
    }

    private List<GameObject> FindMatch(Vector2 castDir)
    { // 1
        List<GameObject> matchingTiles = new List<GameObject>(); // 2
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir); // 3
        while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite)
        { // 4
            matchingTiles.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
        }
        return matchingTiles; // 5
    }

    private void ClearMatch(Vector2[] paths) // 1
    {
        List<GameObject> matchingTiles = new List<GameObject>(); // 2
        for (int i = 0; i < paths.Length; i++) // 3
        {
            matchingTiles.AddRange(FindMatch(paths[i]));
        }
        if (matchingTiles.Count >= 2) // 4
        {
            for (int i = 0; i < matchingTiles.Count; i++) // 5
            {
                matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
                matchingTiles[i].GetComponent<ConnectTile>().Pop.Play();
                HUD.SetPoints(10);
            }
            matchFound = true; // 6
        }
    }

    public void ClearAllMatches(bool where, GameObject otherTile)
    {
        if (render.sprite == null)
            return;

        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        if (matchFound)
        {
            if (otherTile.transform.tag == "Tile" && otherTile != gameObject)
            {
                otherTile.GetComponent<ConnectTile>().noBack = true;
                otherTile.GetComponent<ConnectTile>().swapped = false;
            }
            swapped = false;
            render.sprite = null;
            matchFound = false;
            StopCoroutine(BoardManager.instance.FindNullTiles());
            StartCoroutine(BoardManager.instance.FindNullTiles());
        }
        else
        {
            if (swapped && where && !noBack)
            {
                swapped = false;
                coroutine = SwapBack();
                StartCoroutine(coroutine);
            }
            else
            {
                noBack = false;
            }
        }
    }

    private IEnumerator SwapBack()
    {
        yield return new WaitForSeconds(0.2f);
        if (temp.GetComponent<SpriteRenderer>().sprite == previousImage && render.sprite == swappedImage)
        {
            temp.GetComponent<SpriteRenderer>().sprite = swappedImage;
            render.sprite = previousImage;
        }
        StopCoroutine(BoardManager.instance.FindNullTiles());
        StartCoroutine(BoardManager.instance.FindNullTiles());
        temp = null;
        coroutine = null;
    }
}
