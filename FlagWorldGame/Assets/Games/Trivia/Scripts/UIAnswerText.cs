using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIAnswerText : MonoBehaviour
{
    TextMeshProUGUI answerText;

    void Start()
    {
        answerText = GetComponent<TextMeshProUGUI>();
    }

    public void OnQuestionChange(string newText = null) {
        answerText.SetText($"{newText}");
    }
}
