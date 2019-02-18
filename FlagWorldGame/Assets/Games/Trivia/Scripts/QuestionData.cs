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

    public void SetQuestionData(string questionText = null, QuestionData.QuestionType questionType = 0, params (string answerText, bool isCorrect)[] answerPairs)
    {
        AnswerData[] answersData = new AnswerData[answerPairs.Length];

        AssignAnswerData(answerPairs, answersData);
        SuffleAnswerOrder(ref answersData);

        this.questionText = questionText;
        this.answers = answersData;
        this.type = questionType;
    }

    private void AssignAnswerData((string answerText, bool isCorrect)[] answerPairs, AnswerData[] answersData)
    {
        //cycle answers pairs and assign them to question
        for (int i = 0; i < answerPairs.Length; i++)
        {
            (string answerText, bool isCorrect) pair = answerPairs[i];
            answersData[i] = new AnswerData(pair.answerText, pair.isCorrect);
        }
    }
    private void SuffleAnswerOrder(ref AnswerData[] answerData)
    {
        for (int t = 0; t < answerData.Length; t++) // Randomize the order of answers
        {
            AnswerData tmp = answerData[t];
            int rand = UnityEngine.Random.Range(t, answerData.Length);
            answerData[t] = answerData[rand];
            answerData[rand] = tmp;
        }
    }

}
