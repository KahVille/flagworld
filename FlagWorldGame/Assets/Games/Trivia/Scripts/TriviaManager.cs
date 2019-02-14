using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TriviaManager : MonoBehaviour
{

    ContactPoint dummyContactPoint;
    ContactPoint dummyContactPoint2;

    ContactPointCollection contactPoints;





    [Header("Question Data")]

    //contains questions for current round. 
    [SerializeField]
    private QuestionData[] questions = null;
    private QuestionData currentQuestion = null;
    int currentQuestionNumber = 0;

    [Header("Question UI")]

    [SerializeField]
    private Canvas contiueAndRestartCanvas = null;

    [SerializeField]
    private UIQuestionCanvas questionCanvas = null;

    [SerializeField]
    private GameObject eventSystem= null;

    IEnumerator Start()
    {   
        
        contactPoints = TriviaSaveLoadSystem.LoadContactPoints();
        if(contactPoints == null) {
             yield return StartCoroutine( TriviaSaveLoadSystem.LoadContactPointsFromWeb());
             contactPoints = TriviaSaveLoadSystem.LoadContactPoints();
        }
        //load from file based on the active contact point
        questions = contactPoints.points[0].questions;
        currentQuestion = questions[currentQuestionNumber];
        
        questionCanvas.gameObject.SetActive(true);
        questionCanvas.setNewQuestionUI(currentQuestion);
        yield return true;
    }

    //called on button animation finnished
    public void MoveToNextQuestion()
    {
        eventSystem.SetActive(true);
        if (!LoadNewQuestion())
        {
            Debug.Log("end of round");
            //load end of trivia round canvas
            HideQuestionCanvas();
            ShowContiueAndRestartCanvas();
        }
    }

    private void ShowContiueAndRestartCanvas()
    {
        contiueAndRestartCanvas.enabled = true;
    }

    private void HideQuestionCanvas()
    {
        questionCanvas.gameObject.SetActive(false);
    }

    //load the next question from the question pool
    private bool LoadNewQuestion()
    {
        if (currentQuestionNumber < questions.Length - 1)
        {
            currentQuestionNumber += 1;
            currentQuestion = questions[currentQuestionNumber];
            questionCanvas.setNewQuestionUI(currentQuestion);
            return true;
        }
        else
        {
            return false;
        }

    }

    private QuestionData[] SetDummyRoundData()
    {

        QuestionData dummyQuestionData1 = new QuestionData();
        dummyQuestionData1.SetQuestionData("What is the capital city of Finland", QuestionData.QuestionType.Textonly, ("Helsinki", true), ("Oulu", false), ("Kuopio", false), ("Kotka", false));
        QuestionData dummyQuestionData3 = new QuestionData();
        dummyQuestionData3.SetQuestionData("Does Denmark belong to Nordic Countries", QuestionData.QuestionType.TrueFalse, ("FALSE", false), ("TRUE", true));
        QuestionData dummyQuestionData2 = new QuestionData();
        dummyQuestionData2.SetQuestionData("Press the flag of Finland", QuestionData.QuestionType.Images, ("Finland", true), ("Sweden", false), ("Norway", false), ("Russia", false));
        QuestionData dummyQuestionData4 = new QuestionData();
        dummyQuestionData4.SetQuestionData("Press the flag of Sweden", QuestionData.QuestionType.TrueFalseImage, ("Finland", false), ("Sweden", true));

        return new QuestionData[4] { dummyQuestionData1, dummyQuestionData2, dummyQuestionData3, dummyQuestionData4 };
    }
}
