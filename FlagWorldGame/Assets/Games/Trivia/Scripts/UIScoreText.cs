using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIScoreText : MonoBehaviour
{

    TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Awake()
    {
      scoreText = GetComponent<TextMeshProUGUI>();  
    }


    public void SetTextToDisplay(string textToDisplay=null) {
        scoreText.SetText($"{textToDisplay}");
    }

}
