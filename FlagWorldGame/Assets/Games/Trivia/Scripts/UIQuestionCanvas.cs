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
    private UIAnswerButton[] answerButtons = new UIAnswerButton[4];

    public void setNewQuestionUI(QuestionData question)
    {
        ClearAnswerButtons();

        //set answer buttons by question type
        if (IsImageQuestion(question))
        {
            //set up images to button sprites based on the inputted text from question answertext
            EnableAndSetImageQuestionButtons(question.answers);
        }
        else
        {
            //set Text only Question
            EnableButtonAndSetQuestionTextButtons(question.answers);
        }

        questionTextUI.SetQuestionText(question.questionText + "?");
    }

    public void OnUIAnswerButtonPressed(int option, AnswerData[] answers)
    {

        if(answers[option].isCorrect==true) {
            Debug.Log("UI Correct");
            questionTextUI.SetQuestionText("Correct!");
            answerButtons[option].SetCorrectAnswerColor();
        }
        else 
        {
            questionTextUI.SetQuestionText("Wrong!");
            answerButtons[option].SetWrongAnswerColor();
            for (int i = 0; i < answers.Length; i++)
            {
                if(answers[i].isCorrect == true) {
                    answerButtons[i].SetCorrectAnswerColor();
                }
            }
        }

    }

    private bool IsImageQuestion(QuestionData question)
    {
        if (question.type == QuestionData.QuestionType.Images || question.type == QuestionData.QuestionType.TrueFalseImage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void EnableAndSetImageQuestionButtons(AnswerData[] answers)
    {
        //hacky solution
        for (int i = 0; i < answers.Length; i++)
        {
            int index = System.Array.FindIndex(dummyFlagSprites, sprite => sprite.name == answers[i].answerText);
            answerButtons[i].OnQuestionChange(answers[i], dummyFlagSprites[index]);
        }
    }

    private void EnableButtonAndSetQuestionTextButtons(AnswerData[] answers)
    {
         for (int i = 0; i < answers.Length; i++)
        {
            answerButtons[i].OnQuestionChange(answers[i]);
        }
    }

    private void ClearAnswerButtons()
    {
        //empty answer array 
         for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].ClearButtonData();
        }
    }

}
