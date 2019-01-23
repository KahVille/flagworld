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

        //set up dummy objects
        QuestionData dummyQuestion = new QuestionData();
        AnswerData answerDataDummy = new AnswerData();

        //fill dummy answer data
        answerDataDummy.answerText = "YES";
        answerDataDummy.isCorrect = true;

        //assign answer data to answer array
        AnswerData[] dummyAnswers = new AnswerData[1];
        dummyAnswers[0] = answerDataDummy;


        //fill dummy question data
        dummyQuestion.questionText = "Test Question 1";
        dummyQuestion.answers = dummyAnswers;

        //assign roundData
        QuestionData[] roundData = new QuestionData[1];
        roundData[0] = dummyQuestion;

        return roundData;
    }

}
