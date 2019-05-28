using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestionCanvas : MonoBehaviour
{

    private QuestionData currentQuestion = null;

    [SerializeField]
    private UIQuestionText questionTextUI = null;

    [SerializeField]
    private Sprite[] dummyFlagSprites = null;

    [SerializeField]
    private UIAnswerButton[] answerButtons = new UIAnswerButton[4];

    int score =0;

    public int GetScore () {
        return score;
    }

    public void setNewQuestionUI(QuestionData question)
    {
        currentQuestion = question;
        ClearAnswerButtons();

        currentQuestion.SuffleAnswerOrder();

        //set answer buttons by question type
        if (IsImageQuestion(currentQuestion))
        {
            //set up images to button sprites based on the inputted text from question answertext
            EnableAndSetImageQuestionButtons(currentQuestion.answers);
        }
        else
        {
            //set Text only Question
            EnableButtonAndSetQuestionTextButtons(currentQuestion.answers);
        }
        questionTextUI.SetQuestionText(currentQuestion.questionText);
    }

    public void OnUIAnswerButtonPressed(int option)
    {

        if(currentQuestion.answers[option].isCorrect==true) {
            Debug.Log("UI Correct");
            questionTextUI.SetQuestionText(LocalizationManager.Instance.GetLocalizedValue("correct_choice"));
            answerButtons[option].SetCorrectAnswerColor();
            score+=1;           
        }
        else 
        {
            questionTextUI.SetQuestionText(LocalizationManager.Instance.GetLocalizedValue("wrong_choice"));
            answerButtons[option].SetWrongAnswerColor();
            for (int i = 0; i < currentQuestion.answers.Length; i++)
            {
                if(currentQuestion.answers[i].isCorrect == true) {
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
