using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamesScript : MonoBehaviour
{
    private GameObject[] buttons;
    private Canvas MainM;

    // Start is called before the first frame update
    void Awake()
    {
        buttons = GameObject.FindGameObjectsWithTag("Tile");
        SetButtons();
        MainM = GameObject.Find("MainMenu").GetComponent<Canvas>();
    }

    private void SetButtons()
    {
        int i = 1;
        foreach(GameObject b in buttons)
        {
            if (b.name != "GamesBack")
            {
                b.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -150);
                i++;
            }
        }
    }

    public void ClickEMG()
    {
        SceneManager.LoadScene("EarthMap", LoadSceneMode.Single);
    }

    public void ClickMemory()
    {
        SceneManager.LoadScene("Memory", LoadSceneMode.Single);
    }

    public void ClickSorting()
    {
        SceneManager.LoadScene("Sorting", LoadSceneMode.Single);
    }

    public void ClickTrivia() 
    {
        SceneManager.LoadScene("Trivia", LoadSceneMode.Single);
    } 

    public void ClickBack()
    {
        GetComponent<Canvas>().enabled = false;
        MainM.enabled = true;
    }
}
