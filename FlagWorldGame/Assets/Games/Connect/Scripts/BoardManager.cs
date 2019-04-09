using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public List<Sprite> characters = new List<Sprite>();
    public GameObject tile;
    public int xSize, ySize;

    private GameObject[,] tiles;
    private IEnumerator coroutine;
    private bool updateTip = false;
    private SpriteRenderer Amove;
    private float time, lastMove;
    public float tipInterval = 2f;
    private Color blink = new Color(.5f, .5f, .5f, 1.0f);

    public bool UP = true;
    public bool IsShifting { get; set; }

    void Start()
    {
        instance = GetComponent<BoardManager>();
        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

    private void Update()
    {
        if (UP)
        {
            return;
        }
        time += Time.deltaTime;

        if(time < lastMove + tipInterval || !updateTip)
        {
            return;
        }
        lastMove = time;

        StartCoroutine(Tip());
    }

    private IEnumerator Tip()
    {
        for(int i = 0; i < 2; i++)
        {
            Amove.color = blink;
            yield return new WaitForSeconds(0.2f);
            Amove.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void CreateBoard(float xOffset, float yOffset)
    { // Creates original board for the game.
        tiles = new GameObject[xSize, ySize];

        Sprite[] previousLeft = new Sprite[ySize];
        Sprite previousBelow = null;

        float startX = transform.position.x;
        float startY = transform.position.y;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<Sprite> possibleCharacters = new List<Sprite>();
                possibleCharacters.AddRange(characters);

                possibleCharacters.Remove(previousLeft[y]);
                possibleCharacters.Remove(previousBelow);

                Sprite newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                previousLeft[y] = newSprite;
                previousBelow = newSprite;
            }
        }

        CheckForMatches();
        lastMove = time;
        updateTip = true;
    }

    public IEnumerator FindNullTiles() // Looks for empty spaces on the board.
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    coroutine = ShiftTilesDown(x, y);
                    yield return StartCoroutine(coroutine);
                    break;
                }
            }
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                tiles[x, y].GetComponent<ConnectTile>().ClearAllMatches(false, gameObject);
            }
        }

        CheckNull();
        CheckForMatches();
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .03f) // Creates falling effect on new tiles.
    {
        IsShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;


        if (yStart == 11)
        {
            SpriteRenderer render = tiles[x, yStart].GetComponent<SpriteRenderer>();
            render.sprite = GetNewSprite(x, ySize - 1);
            IsShifting = false;
        }
        else
        {
            for (int y = yStart; y < ySize; y++)
            {  // 1
                SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
                if (render.sprite == null)
                { // 2
                    nullCount++;
                }
                renders.Add(render);
            }

            for (int i = 0; i < nullCount; i++)
            { // 3
                yield return new WaitForSeconds(shiftDelay);
                for (int k = 0; k < renders.Count - 1; k++)
                { // 5
                    renders[k].sprite = renders[k + 1].sprite;
                    //renders[k + 1].sprite = GetNewSprite(x, ySize - 1);
                    renders[k + 1].sprite = characters[Random.Range(0, characters.Count)];
                }
            }
            IsShifting = false;
        }

        coroutine = null;
    }

    private Sprite GetNewSprite(int x, int y) // Randomizes sprites and makes sure there isn't instantly matches.
    {
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        if (x > 0)
        {
            possibleCharacters.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1)
        {
            possibleCharacters.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleCharacters.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
    }

    private void CheckForMatches() // Check if board has possible moves.
    {
        lastMove = time;
        for (int x = 1; x < xSize - 1; x++)
        {
            for (int y = 1; y < ySize - 1; y++)
            {
                #region Take every tile 3x3 from center [x,y]
                Sprite ULCorner = tiles[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite;
                Sprite UCenter = tiles[x, y + 1].GetComponent<SpriteRenderer>().sprite;
                Sprite URCorner = tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite;
                Sprite LCenter = tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite;
                Sprite Center = tiles[x, y].GetComponent<SpriteRenderer>().sprite;
                Sprite RCenter = tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite;
                Sprite DLCorner = tiles[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite;
                Sprite DCenter = tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite;
                Sprite DRCorner = tiles[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite;
                #endregion

                #region Check each row
                if (ULCorner == UCenter || UCenter == URCorner || ULCorner == URCorner) // if upper row has two of same
                {
                    if (ULCorner == UCenter)
                    {
                        if (ULCorner == RCenter)
                        {
                            Amove = tiles[x + 1, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (x != xSize - 2 && ULCorner == tiles[x + 2, y + 1].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x + 2, y + 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else if (UCenter == URCorner)
                    {
                        if (URCorner == LCenter)
                        {
                            Amove = tiles[x - 1, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (x != 1 && URCorner == tiles[x - 2, y + 1].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x - 2, y + 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else // ULCorner == URCorner
                    {
                        if (ULCorner == Center)
                        {
                            Amove = tiles[x, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                }
                if (LCenter == Center || Center == RCenter || LCenter == RCenter) // if middle row has two of same
                {
                    if (LCenter == Center)
                    {
                        if (LCenter == URCorner || LCenter == DRCorner)
                        {
                            if(LCenter == URCorner)
                            {
                                Amove = tiles[x + 1, y + 1].GetComponent<SpriteRenderer>();
                            }
                            else
                            {
                                Amove = tiles[x + 1, y - 1].GetComponent<SpriteRenderer>();
                            }

                            return;
                        }
                        else if (x != xSize - 2 && Center == tiles[x + 2, y].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x + 2, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else if (Center == RCenter)
                    {
                        if (Center == ULCorner || Center == DLCorner)
                        {
                            if (Center == ULCorner)
                            {
                                Amove = tiles[x - 1, y + 1].GetComponent<SpriteRenderer>();
                            }
                            else
                            {
                                Amove = tiles[x - 1, y - 1].GetComponent<SpriteRenderer>();
                            }
                            return;
                        }
                        else if (x != 1 && Center == tiles[x - 2, y].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x - 2, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else // LCenter == RCenter
                    {
                        if (LCenter == UCenter || LCenter == DCenter)
                        {
                            if (LCenter == UCenter)
                            {
                                Amove = tiles[x, y + 1].GetComponent<SpriteRenderer>();
                            }
                            else
                            {
                                Amove = tiles[x, y - 1].GetComponent<SpriteRenderer>();
                            }
                            return;
                        }
                    }
                }
                if (DLCorner == DCenter || DCenter == DRCorner || DLCorner == DRCorner) // if bottom row has two of same
                {
                    if (DLCorner == DCenter)
                    {
                        if (DLCorner == RCenter)
                        {
                            Amove = tiles[x + 1, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (x != xSize - 2 && DLCorner == tiles[x + 2, y - 1].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x + 2, y - 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else if (DCenter == DRCorner)
                    {
                        if (DRCorner == LCenter)
                        {
                            Amove = tiles[x - 1, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (x != 1 && DRCorner == tiles[x - 2, y - 1].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x - 2, y - 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else // DLCorner == DRCorner
                    {
                        if (DRCorner == Center)
                        {
                            Amove = tiles[x, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                }
                #endregion

                #region Check each column
                if (ULCorner == LCenter || LCenter == DLCorner || ULCorner == DLCorner) // left column
                {
                    if (ULCorner == LCenter)
                    {
                        if (ULCorner == DCenter)
                        {
                            Amove = tiles[x, y - 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (y != 1 && ULCorner == tiles[x - 1, y - 2].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x - 1, y - 2].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else if (LCenter == DLCorner)
                    {
                        if (DLCorner == UCenter)
                        {
                            Amove = tiles[x, y + 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (y != ySize - 2 && DLCorner == tiles[x - 1, y + 2].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x - 1, y + 2].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else // ULCorner == DLCorner
                    {
                        if (ULCorner == Center)
                        {
                            Amove = tiles[x, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                }
                if (UCenter == Center || Center == DCenter || UCenter == DCenter) // center column
                {
                    if (UCenter == Center)
                    {
                        if (UCenter == DLCorner || UCenter == DRCorner)
                        {
                            if (UCenter == DLCorner)
                            {
                                Amove = tiles[x - 1, y - 1].GetComponent<SpriteRenderer>();
                            }
                            else
                            {
                                Amove = tiles[x + 1, y - 1].GetComponent<SpriteRenderer>();
                            }
                            return;
                        }
                        else if (y != 1 && Center == tiles[x, y - 2].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x, y - 2].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else if (Center == DCenter)
                    {
                        if (Center == ULCorner || Center == URCorner)
                        {
                            if (Center == ULCorner)
                            {
                                Amove = tiles[x - 1, y + 1].GetComponent<SpriteRenderer>();
                            }
                            else
                            {
                                Amove = tiles[x + 1, y + 1].GetComponent<SpriteRenderer>();
                            }
                            return;
                        }
                        else if (y != ySize - 2 && DCenter == tiles[x, y + 2].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x, y + 2].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else // UCenter == DCenter
                    {
                        if (UCenter == LCenter || UCenter == RCenter)
                        {
                            if (UCenter == LCenter)
                            {
                                Amove = tiles[x - 1, y].GetComponent<SpriteRenderer>();
                            }
                            else
                            {
                                Amove = tiles[x + 1, y].GetComponent<SpriteRenderer>();
                            }
                            return;
                        }
                    }
                }
                if (URCorner == RCenter || RCenter == DRCorner || URCorner == DRCorner) // right column
                {
                    if (URCorner == RCenter)
                    {
                        if (URCorner == DCenter)
                        {
                            Amove = tiles[x, y - 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (y != 1 && URCorner == tiles[x + 1, y - 2].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x + 1, y - 2].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else if (RCenter == DRCorner)
                    {
                        if (DRCorner == UCenter)
                        {
                            Amove = tiles[x, y + 1].GetComponent<SpriteRenderer>();
                            return;
                        }
                        else if (y != ySize - 2 && DRCorner == tiles[x + 1, y + 2].GetComponent<SpriteRenderer>().sprite)
                        {
                            Amove = tiles[x + 1, y + 2].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                    else // URCorner == DRCorner
                    {
                        if (URCorner == Center)
                        {
                            Amove = tiles[x, y].GetComponent<SpriteRenderer>();
                            return;
                        }
                    }
                }
                #endregion
            }
        }

        updateTip = false;
        StartCoroutine(ReDoBoard()); // if functions get's to this point, there are no moves left
    }

    private IEnumerator ReDoBoard() // empties board.
    {
        yield return new WaitForSeconds(0.2f);
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                tiles[x, y].GetComponent<SpriteRenderer>().sprite = null;
            }
        }

        CheckNull();
    }

    private void CheckNull() // Same function as FindNullTiles(), but not coroutine to be used differently.
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    if (coroutine == null)
                    {
                        StartCoroutine(ShiftTilesDown(x, y));
                        break;
                    }
                }
            }
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                tiles[x, y].GetComponent<ConnectTile>().ClearAllMatches(false, gameObject);
            }
        }

        StartCoroutine(WaitToCheck());
    }

    private IEnumerator WaitToCheck()
    {
        yield return new WaitForSeconds(2f);
        for (int x = 1; x < xSize - 1; x++)
        {
            for (int y = 1; y < ySize - 1; y++)
            {
                tiles[x, y].GetComponent<ConnectTile>().ClearAllMatches(true, tiles[x, y]);
            }
        }
        updateTip = true;
        CheckForMatches();
    }

}
