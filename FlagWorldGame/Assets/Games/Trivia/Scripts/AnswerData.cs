using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnswerData 
{
    public string answerText;
    public bool isCorrect;

    public AnswerData(string text = null, bool correct = false) {
        answerText = text;
        isCorrect = correct;
    }
   
}
