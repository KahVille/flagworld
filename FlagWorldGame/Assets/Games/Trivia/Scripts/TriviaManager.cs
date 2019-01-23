using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriviaManager : MonoBehaviour
{
    //contains questions for current round. 
    [SerializeField]
    private QuestionData[] currentRoundQuestions;
    int currentQuestionNumber=0;

    QuestionData currentQuestion;
    [SerializeField]
    private UIAnswerText[] AnswerTexts = new UIAnswerText[4];
    [SerializeField]
    private UIQuestionText questionTextUI = null;


    void Start()
    {
      currentRoundQuestions = LoadRoundQuestions(0);
      currentQuestion = currentRoundQuestions[currentQuestionNumber];
      setNewQuestionUI(currentQuestion);
    }

    void setNewQuestionUI(QuestionData question) {
        for (int i =0; i <= question.answers.Length -1; i++) {
            AnswerTexts[i].OnQuestionChange(question.answers[i].answerText);
        }
        questionTextUI.SetQuestionText(question.questionText);
    }

    public void OnAnswerButtonPressed(int option) {
            if(currentQuestion.answers[option].isCorrect == true)
        {
            Debug.Log("Correct Answer, give points");
            //give points.            
        }
        if(!LoadNewQuestion())
                Debug.Log("end of round");
    }

    private bool LoadNewQuestion()
    {
        if(currentQuestionNumber < currentRoundQuestions.Length-1) {
            currentQuestionNumber += 1;
            currentQuestion = currentRoundQuestions[currentQuestionNumber];
            setNewQuestionUI(currentQuestion);
            return true;
        }
        else {
            return false;
        }

    }

    public QuestionData[] LoadRoundQuestions(int roundID) {

        QuestionData dummyQuestionData1 = new QuestionData();
        QuestionData dummyQuestionData2 = new QuestionData();
        QuestionData dummyQuestionData3 = new QuestionData();

        AnswerData dummyAnswerData1 = new AnswerData();
        dummyAnswerData1.answerText = "option 1";
        dummyAnswerData1.isCorrect = true;
        AnswerData dummyAnswerData2 = new AnswerData();
        dummyAnswerData2.answerText = "option 2";
        dummyAnswerData2.isCorrect = false;
        AnswerData dummyAnswerData3 = new AnswerData();
        dummyAnswerData3.answerText = "option 3";
        dummyAnswerData3.isCorrect = false;
        AnswerData dummyAnswerData4 = new AnswerData();
        dummyAnswerData4.answerText = "option 4";
        dummyAnswerData4.isCorrect = false;

        AnswerData[] dummyAnswers = new AnswerData[4] {dummyAnswerData1,dummyAnswerData2, dummyAnswerData3, dummyAnswerData4};


        dummyQuestionData1.questionText = "Test Question 1";
        dummyQuestionData1.answers = dummyAnswers;

        dummyQuestionData2.questionText = "Test Question 2";
        dummyQuestionData2.answers = dummyAnswers;

        dummyQuestionData3.questionText = "Test Question 3";
        dummyQuestionData3.answers = dummyAnswers;

        //assign roundData
        QuestionData[] roundData = new QuestionData[3] {dummyQuestionData1, dummyQuestionData2, dummyQuestionData3};

        return roundData;
    }

}
