using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TriviaManager : MonoBehaviour
{

    [Header("Question Data")]

    //contains questions for current round. 
    [SerializeField]
    private QuestionData[] currentRoundQuestions;
    int currentQuestionNumber = 0;

    QuestionData currentQuestion;


    [Header("Question UI")]

    [SerializeField]
    private UIQuestionText questionTextUI = null;

    [SerializeField]
    private UIScoreText scoreTextUI = null;

    [SerializeField]
    private Canvas answerButtonsCanvas = null;

    [SerializeField]
    private Canvas contiueAndRestartCanvas = null;

    [SerializeField]
    private UIQuestionCanvas questionCanvas = null;

    [SerializeField]
    private GameObject eventSystem;

    int roundScore = 0;
    void Start()
    {
        roundScore = 0;
        currentRoundQuestions = LoadRoundQuestions(0);
        currentQuestion = currentRoundQuestions[currentQuestionNumber];
        questionCanvas.setNewQuestionUI(currentQuestion);
    }

    public void OnAnswerButtonPressed(int option)
    {

        questionCanvas.OnUIAnswerButtonPressed(option, currentQuestion.answers);
        if (currentQuestion.answers[option].isCorrect == true)
        {
            eventSystem.SetActive(false);
            Debug.Log("Correct Answer, give points");
            roundScore += 10;
            scoreTextUI.SetTextToDisplay(roundScore.ToString() + " FP");
            //set Button with option, Color To Green
        }


    }


    //called on button animation finnished
    public void MoveToNextQuestion() {
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
        if (currentQuestionNumber < currentRoundQuestions.Length - 1)
        {
            currentQuestionNumber += 1;
            currentQuestion = currentRoundQuestions[currentQuestionNumber];
            questionCanvas.setNewQuestionUI(currentQuestion);
            return true;
        }
        else
        {
            return false;
        }

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
        QuestionData dummyQuestionData4 = new QuestionData();

        dummyQuestionData1 = SetQuestionData("What is the capital city of Finland", QuestionData.QuestionType.Textonly, ("Helsinki", true), ("Oulu", false), ("Kuopio", false), ("Kotka", false));
        dummyQuestionData3 = SetQuestionData("Does Denmark belong to Nordic Countries", QuestionData.QuestionType.TrueFalse, ("FALSE", false), ("TRUE", true));
        dummyQuestionData2 = SetQuestionData("Press the flag of Finland", QuestionData.QuestionType.Images, ("Finland", true), ("Sweden", false), ("Norway", false), ("Russia", false));
        dummyQuestionData4 = SetQuestionData("Press the flag of Sweden", QuestionData.QuestionType.TrueFalseImage, ("Finland", false), ("Sweden", true));

        return new QuestionData[4] { dummyQuestionData1, dummyQuestionData2, dummyQuestionData3, dummyQuestionData4 };
    }
    private AnswerData SetAnswerData(string text = null, bool correct = false)
    {
        AnswerData answer = new AnswerData();
        answer.answerText = text;
        answer.isCorrect = correct;
        return answer;
    }

    private QuestionData SetQuestionData(string questionText = null, QuestionData.QuestionType questionType = 0, params (string answerText, bool isCorrect)[] answerPairs)
    {
        QuestionData questionData = new QuestionData();
        AnswerData[] answersData = new AnswerData[answerPairs.Length];

        questionData.questionText = questionText;
        questionData.type = questionType;

        AssignAnswerData(answerPairs, answersData);
        SuffleAnswerOrder(ref answersData);

        questionData.answers = answersData;

        return questionData;
    }

    private void AssignAnswerData((string answerText, bool isCorrect)[] answerPairs, AnswerData[] answersData)
    {
        //cycle answers pairs and assign them to question
        for (int i = 0; i < answerPairs.Length; i++)
        {
            (string answerText, bool isCorrect) pair = answerPairs[i];
            answersData[i] = SetAnswerData(pair.answerText, pair.isCorrect);
        }
    }

    private static void SuffleAnswerOrder(ref AnswerData[] answerData)
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
