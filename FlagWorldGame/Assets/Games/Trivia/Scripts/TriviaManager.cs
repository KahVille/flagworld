using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TriviaManager : MonoBehaviour
{
    //contains questions for current round. 
    [SerializeField]
    private QuestionData[] currentRoundQuestions;
    int currentQuestionNumber = 0;

    QuestionData currentQuestion;
    [SerializeField]
    private UIAnswerText[] AnswerTexts = new UIAnswerText[4];
    [SerializeField]
    private UIQuestionText questionTextUI = null;

    [SerializeField]
    private UIScoreText scoreTextUI = null;

    [SerializeField]
    private Canvas answerButtonsCanvas = null;

    [SerializeField]
    private Canvas contiueAndRestartCanvas = null;

    int roundScore = 0;


    void Start()
    {
        roundScore = 0;
        currentRoundQuestions = LoadRoundQuestions(0);
        currentQuestion = currentRoundQuestions[currentQuestionNumber];
        setNewQuestionUI(currentQuestion);
    }

    void setNewQuestionUI(QuestionData question)
    {
        for (int i = 0; i <= question.answers.Length - 1; i++)
        {
            AnswerTexts[i].OnQuestionChange(question.answers[i].answerText);
        }
        questionTextUI.SetQuestionText(question.questionText + "?");
    }

    public void OnAnswerButtonPressed(int option)
    {
        if (currentQuestion.answers[option].isCorrect == true)
        {
            Debug.Log("Correct Answer, give points");
            roundScore += 10;
            scoreTextUI.SetTextToDisplay(roundScore.ToString() + " FP");
        }
        if (!LoadNewQuestion())
        {
            Debug.Log("end of round");
            //load end of trivia round canvas
            questionTextUI.SetQuestionText("Round Ended, You scored:");
            HideAnswerButtons();
            ShowContiueAndRestartCanvas();

        }
    }

    private void ShowContiueAndRestartCanvas()
    {
        contiueAndRestartCanvas.enabled = true;
    }

    private void HideAnswerButtons()
    {
        answerButtonsCanvas.enabled = false;
    }

    private bool LoadNewQuestion()
    {
        if (currentQuestionNumber < currentRoundQuestions.Length - 1)
        {
            currentQuestionNumber += 1;
            currentQuestion = currentRoundQuestions[currentQuestionNumber];
            setNewQuestionUI(currentQuestion);
            return true;
        }
        else
        {
            return false;
        }

    }

    private AnswerData SetAnswerData(string text = null, bool correct = false)
    {
        AnswerData answer = new AnswerData();
        answer.answerText = text;
        answer.isCorrect = correct;
        return answer;
    }

    public QuestionData[] LoadRoundQuestions(int roundID)
    {
        //assign roundData
        QuestionData[] roundData = SetDummyRoundData();

        return roundData;
    }

    private QuestionData[] SetDummyRoundData()
    {
        QuestionData dummyQuestionData1 = new QuestionData();
        QuestionData dummyQuestionData2 = new QuestionData();
        QuestionData dummyQuestionData3 = new QuestionData();


        dummyQuestionData1.questionText = "What is the capital city of Finland";
        AnswerData dummyAnswerData1 = SetAnswerData("Helsinki", true);
        AnswerData dummyAnswerData2 = SetAnswerData("Oulu");
        AnswerData dummyAnswerData3 = SetAnswerData("Turku");
        AnswerData dummyAnswerData4 = SetAnswerData("Kotka");
        AnswerData[] dummyAnswers = new AnswerData[4] { dummyAnswerData1, dummyAnswerData2, dummyAnswerData3, dummyAnswerData4 };
        dummyQuestionData1.answers = dummyAnswers;


        dummyQuestionData2.questionText = "What is the capital city of Sweden";

        dummyAnswerData1 = SetAnswerData("Malmö");
        dummyAnswerData2 = SetAnswerData("Stockholm", true);
        dummyAnswerData3 = SetAnswerData("Lund");
        dummyAnswerData4 = SetAnswerData("Motala");
        dummyAnswers = new AnswerData[4] { dummyAnswerData1, dummyAnswerData2, dummyAnswerData3, dummyAnswerData4 };
        dummyQuestionData2.answers = dummyAnswers;


        dummyQuestionData3.questionText = "What is the capital city of Norway";
        dummyAnswerData1 = SetAnswerData("Mysen");
        dummyAnswerData2 = SetAnswerData("Kopervik");
        dummyAnswerData3 = SetAnswerData("Oslo", true);
        dummyAnswerData4 = SetAnswerData("Odda");
        dummyAnswers = new AnswerData[4] { dummyAnswerData1, dummyAnswerData2, dummyAnswerData3, dummyAnswerData4 };
        dummyQuestionData3.answers = dummyAnswers;

        return new QuestionData[3] { dummyQuestionData1, dummyQuestionData2, dummyQuestionData3 };

    }

}
