using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuistiManager : MonoBehaviour {

    public Sprite[] pictures, pairs;
    public GameObject tile;
    public int xSize, ySize;

    private Sprite[] boardPics;
    private GameObject[,] tiles;
    private int moves = 0;

	void Start () {
        if (xSize * ySize == pictures.Length * 2) {
            SetBoardPics();
            Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
            CreateBoard(offset.x, offset.y);
        }
	}

    private void SetBoardPics()
    {
        boardPics = new Sprite[pictures.Length * 2];
        int bP = 0;

        for(int i = 0; i < pictures.Length; i++)
        {
            boardPics[bP] = pictures[i];
            bP++;
        }
        for(int i = 0; i < pairs.Length; i++)
        {
            boardPics[bP] = pairs[i];
            bP++;
        }

        for (int t = 0; t < boardPics.Length; t++) // Randomize the order of Sprite within array
        {
            Sprite tmp = boardPics[t];
            int r = Random.Range(t, boardPics.Length);
            boardPics[t] = boardPics[r];
            boardPics[r] = tmp;
        }
    }

    private void CreateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[xSize, ySize];

        float startX = transform.position.x;
        float startY = transform.position.y;
        int bP = 0;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                newTile.GetComponent<MuistiTile>().SetPicture(boardPics[bP]);
                newTile.GetComponent<MuistiTile>().SetPair(GetPair(boardPics[bP]));
                bP++;
            }
        }
    }

    private void CheckBoardPairs()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if(tiles[x, y].GetComponent<MuistiTile>().Paired == false)
                {
                    return;
                }
            }
        }
        tiles[0, 0].GetComponent<MuistiTile>().StartPressed();
        StartCoroutine(EndGame(true));
    }

    private int CountPairs()
    {
        int pairs = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (tiles[x, y].GetComponent<MuistiTile>().Paired == true)
                {
                    pairs++;
                }
            }
        }
        pairs = pairs / 2;
        return pairs;
    }

    private IEnumerator EndGame(bool fin)
    {
        GameObject.Find("Stop").SetActive(false);
        yield return new WaitForSeconds(2);
        GameObject.Find("StartCanvas").GetComponent<MuistiCanvas>().TheEnd(moves, fin, pictures.Length);
 
    }

    public void GetMove()
    {
        moves++;
        CheckBoardPairs();
    }

    public void ClickStop()
    {
        tiles[0, 0].GetComponent<MuistiTile>().StartPressed();
        GameObject.Find("StartCanvas").GetComponent<MuistiCanvas>().TheEnd(moves, false, CountPairs());
    }

    private Sprite GetPair(Sprite pic)
    {
        Sprite pair = null;
        bool found = false;
        for (int i = 0; i < pictures.Length; i++)
        {
            if (pictures[i] == pic)
            {
                pair = pairs[i];
                found = true;
                break;
            }       
        }
        if (!found)
        {
            for (int i = 0; i < pairs.Length; i++)
            {
                if (pairs[i] == pic)
                {
                    pair = pictures[i];
                    break;
                }
            }
        }

        return pair;
    }
}
