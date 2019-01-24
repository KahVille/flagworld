using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuestionData 
{
    public enum QuestionType
    {
        Textonly,
        TrueFalse,
        TrueFalseImage,
        Images,
    }

    public string questionText;
    public AnswerData[] answers;
    public QuestionType type;
}
