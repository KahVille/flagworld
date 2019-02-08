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

    public QuestionData(string text = null, AnswerData[] answerdata = null, QuestionType questionType = 0) {
        questionText = text;
        answers = answerdata;
        type = questionType;
    }

}
