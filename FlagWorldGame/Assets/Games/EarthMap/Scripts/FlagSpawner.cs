using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlagSpawner : MonoBehaviour
{
    public List<Sprite> flagsImgs = new List<Sprite>();
    public List<GameObject> flagObjs = new List<GameObject>();
    int currentIndex = 0;
    public GameObject flagObj;

    public Transform countryColParent;
    public List<GameObject> countryObjs = new List<GameObject>();

    Coroutine checkAnswersCO;
    PlayerScore psScript;
    public GameObject scoreText;
    
    void Start()
    {
        Sprite temp;
        // Randomize image list (Fisher-Yates shuffle)
        for(int i = 0; i < flagsImgs.Count; i++)
        {
            int random = Random.Range(0, i+1);
            temp = flagsImgs[random];
            flagsImgs[random] = flagsImgs[i];
            flagsImgs[i] = temp;
        }

        foreach (Transform child in countryColParent.transform)
        {
            countryObjs.Add(child.gameObject);
            //countryCols.Add(child.GetComponent<PolygonCollider2D>());
            //child.GetComponent<PolygonCollider2D>().enabled = false;
        }


        psScript = FindObjectOfType<PlayerScore>();
        SpawnFlag();
    }

    public void SpawnFlag()
    {
        if(currentIndex < flagsImgs.Count)
        {
            GameObject newFlag = Instantiate(flagObj, transform);
            newFlag.GetComponent<Image>().sprite = flagsImgs[currentIndex];
            newFlag.name = flagsImgs[currentIndex].name.Replace("Flag", "");
            flagObjs.Add(newFlag);
            Debug.Log(newFlag.transform.position);
            currentIndex++;
        }
    }

    public GameObject GetCurrentFlag()
    {
        return flagObjs[currentIndex-1];
    }

    public void StartCheckAnswers()
    {
        if(checkAnswersCO == null)
        {
            checkAnswersCO = StartCoroutine(CheckAnswers());
        }
    }

    IEnumerator CheckAnswers()
    {
        // foreach (PolygonCollider2D col in countryCols)
        // {
        //     col.enabled = true;
        // }
        // yield return new WaitForEndOfFrame();
        // foreach (PolygonCollider2D col in countryCols)
        // {
        //     col.enabled = false;
        // }
        for(int i = 0; i < countryObjs.Count; i++)
        {
            for(int j = 0; j < flagObjs.Count; j++)
            {
                Debug.Log("Country: " + countryObjs[i].GetComponent<PolygonCollider2D>().bounds.Contains((Vector2)flagObjs[j].transform.position));
                //Debug.Log("Flag: " + flagObjs[j].transform.position);
                if(countryObjs[i].GetComponent<PolygonCollider2D>().bounds.Contains((Vector2)flagObjs[j].transform.position) 
                && countryObjs[i].transform.name == flagObjs[j].transform.name)
                {
                    psScript.AddScore();
                }
            }
        }

        scoreText.SetActive(true);
        scoreText.GetComponent<TextMeshProUGUI>().text = psScript.FlagPoints.ToString();
        yield return null;
    }
}
