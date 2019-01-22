using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriviaManager : MonoBehaviour
{
    //contains questions for current round. 
    public QuestionData[] currentRoundQuestions;

    void Start()
    {
      currentRoundQuestions = LoadRoundQuestions(0); 
    }

    public QuestionData[] LoadRoundQuestions(int roundID) {
        QuestionData dummyQuestion = new QuestionData();
        AnswerData answerDataDummy = new AnswerData();

        
        answerDataDummy.answerText = "YES";
        answerDataDummy.isCorrect = true;
        AnswerData[] dummyAnswers = new AnswerData[1];
        dummyAnswers[0] = answerDataDummy;

        dummyQuestion.questionText = "Test Question 1";
        dummyQuestion.answers = dummyAnswers;



        QuestionData[] roundData = new QuestionData[1];
        roundData[0] = dummyQuestion;

        //roundData[1].questionText = "Test Question 2?";
        //roundData[1].answers = dummyAnswers;
        //roundData[2].questionText = "Test Question 3?";
        //roundData[3].answers = dummyAnswers;
        return roundData;
    }

}
