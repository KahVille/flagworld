using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIQuestionText : MonoBehaviour
{

    TextMeshProUGUI questionText;

    // Start is called before the first frame update
    void Start()
    {
       questionText = GetComponent<TextMeshProUGUI>(); 
    }


    public void SetQuestionText(string textToSet = null) {
        questionText.SetText($"{textToSet}");
    }


}
