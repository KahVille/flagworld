using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestionCanvas : MonoBehaviour
{

    [SerializeField]
    private UIQuestionText questionTextUI = null;

    [SerializeField]
    private Sprite[] dummyFlagSprites = null;
    
    [SerializeField]
    private UIAnswerText[] AnswerTexts = new UIAnswerText[4];

    // Start is called before the first frame update

public void setNewQuestionUI(QuestionData question)
    {
        ClearAnswerButtons();

        //set answer buttons by question type
        if (IsImageQuestion(question))
        {
            //set up images to button sprites based on the inputted text from question answertext
            EnableAndSetImageQuestion(question);
        }
        else
        {
            //set Text only Question
            EnableButtonAndSetQuestionText(question);
        }

        questionTextUI.SetQuestionText(question.questionText + "?");
    }

        private bool IsImageQuestion(QuestionData question) {
        if (question.type == QuestionData.QuestionType.Images || question.type == QuestionData.QuestionType.TrueFalseImage) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private void EnableAndSetImageQuestion(QuestionData question)
    {
        //hacky solution
        for (int i = 0; i < question.answers.Length; i++)
        {
            int index = System.Array.FindIndex(dummyFlagSprites, sprite => sprite.name == question.answers[i].answerText);
            AnswerTexts[i].OnQuestionChange(question.answers[i],dummyFlagSprites[index]);
        }
    }

    private void EnableButtonAndSetQuestionText(QuestionData question)
    {
        for (int i = 0; i <= question.answers.Length - 1; i++)
        {
            AnswerTexts[i].OnQuestionChange(question.answers[i]);
        }
    }

    private void ClearAnswerButtons()
    {
        //empty answer array 
        foreach (var item in AnswerTexts)
        {
            item.ClearButtonData();
        }
    }

}
