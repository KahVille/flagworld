﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class TriviaManager : MonoBehaviour
{

    int maxQuestions = 5;
    int maxRandomQs = 10;

    [SerializeField]
    private UITriviaCanvas triviaCanvas = null;

    ContactPointCollection contactPoints;
    private int currentContactPointIndex = 1;

    [Header("Question Data")]

    //contains questions for current round. 
    [SerializeField]
    private QuestionData[] questions = null;
    private QuestionData currentQuestion = null;
    private int currentQuestionNumber = 0;

    [Header("Question UI")]

    [SerializeField]
    private UIQuestionCanvas questionCanvas = null;

    [SerializeField]
    UIScoreText scoreText = null;

    private ModalPanel modalPanel;
    public Text test;
    public RectTransform QuestionLabelB;

    void Awake()
    {
        modalPanel = ModalPanel.Instance();

        if (Screen.height / Screen.width <= 1.5f)
        {
            QuestionLabelB.sizeDelta = new Vector2(1000, 300);
            QuestionLabelB.anchoredPosition = new Vector3(0, 500, 0);
        }
    }

    private void SetScoreText()
    {
        scoreText.SetTextToDisplay($"{currentQuestionNumber + 1 } / {questions.Length}");
    }

    public void ShowPanel(string title = null, string desc = null)
    {
        EventButtonDetails button1Detail = new EventButtonDetails { buttonTitle = "Back to Title", action = ClosePanel };
        SpawnPanel(title, desc, button1Detail);
    }

    void SpawnPanel(string mainTitle, string titleDescription, EventButtonDetails button1, EventButtonDetails button2 = null, Sprite icon = null)
    {
        ModalPanelDetails modalPanelDetails = new ModalPanelDetails { shortText = mainTitle, description = titleDescription, iconImage = icon };
        modalPanelDetails.button1Details = button1;
        modalPanelDetails.button2Details = button2;
        modalPanel.SpawnWithDetails(modalPanelDetails);
    }

    public void ClosePanel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    IEnumerator Start()
    {
        triviaCanvas.EnableLoadIndicator();
        contactPoints = TriviaSaveLoadSystem.LoadContactPoints();
        if (contactPoints == null)
        {
            //unity error
            triviaCanvas.DisableLoadIndicator();
            questions = SetDummyRoundData();
            SuffleQuestionOrder(ref questions);
            currentQuestion = questions[currentQuestionNumber];
            EnableQuestionCanvas();
            SetScoreText();
            questionCanvas.setNewQuestionUI(currentQuestion);

        }
        else
        {
            int questionAmount = maxQuestions;
            triviaCanvas.DisableLoadIndicator();
            //id that is inside the contact point;contactpoint.identifier

            if (PlayerPrefs.GetInt("TriviaAllRandom", 0) == 1)
            {
                //reset all random state
                PlayerPrefs.SetInt("TriviaAllRandom", 0);
                AllRandomAllQuestions();
                questionAmount = maxRandomQs;
            }
            else
            {
                currentContactPointIndex = SelectContactPointIndex(PlayerPrefs.GetInt("CurrentLocationIdentifier", 0));
                questions = contactPoints.points[currentContactPointIndex].questions;
            }

            DisplayCurrentQuestionAndEnableCanvas(questionAmount);
        }

        yield return true;
    }

    //suffle contact point order
    //select first 3 contact points
    //suffle each of the contact point questions
    //assign contact point first question set to the random pool
    void AllRandomAllQuestions()
    {
        // Take all questions into a list 
        List<QuestionData> QuestionsPool = new List<QuestionData>(); 
        int size = 0;

        foreach(var point in contactPoints.points)
        {
            for (int i = 0; i < point.questions.Length; i++)
            {
                QuestionsPool.Add(point.questions[i]);
                size++;
            }
        }

        // Suffle questions 
        QuestionData[] AllRandomQuestions = new QuestionData[size];
        AllRandomQuestions = QuestionsPool.ToArray();
        SuffleQuestionOrder(ref AllRandomQuestions);

        // Add first 5 questions to game
        QuestionData[] tmp = new QuestionData[maxRandomQs];

        for(int i = 0; i < maxRandomQs; i++)
        {
            tmp[i] = AllRandomQuestions[i];
        }

        questions = tmp;


    }

    private void SuffleContactPointOrder(ref ContactPoint[] contactPointsData)
    {
        for (int t = 0; t < contactPointsData.Length; t++) // Randomize the order of answers
        {
            ContactPoint tmp = contactPointsData[t];
            int rand = UnityEngine.Random.Range(t, contactPointsData.Length);
            contactPointsData[t] = contactPointsData[rand];
            contactPointsData[rand] = tmp;
        }
    }


    private void SuffleQuestionOrder(ref QuestionData[] questionData)
    {
        if (questionData.Length < maxQuestions)
        {
            for (int t = 0; t < questionData.Length; t++) // Randomize the order of answers
            {
                QuestionData tmp = questionData[t];
                int rand = UnityEngine.Random.Range(t, questionData.Length);
                questionData[t] = questionData[rand];
                questionData[rand] = tmp;
            }
        }
        else
        {
            for (int t = 0; t < maxQuestions; t++) // Randomize the order of answers
            {
                QuestionData tmp = questionData[t];
                int rand = UnityEngine.Random.Range(t, questionData.Length);
                questionData[t] = questionData[rand];
                questionData[rand] = tmp;
            }
        }
    }

    private void DisplayCurrentQuestionAndEnableCanvas(int questionAmount)
    {

        SuffleQuestionOrder(ref questions);

        //limit the questions to number of max questions
        if (questions.Length > questionAmount)
        {
            QuestionData[] tmp = new QuestionData[questionAmount];
            for (int i = 0; i < questionAmount; i++)
            {
                tmp[i] = questions[i];
            }
            questions = tmp;
        }

        currentQuestion = questions[currentQuestionNumber];
        EnableQuestionCanvas();
        SetScoreText();
        questionCanvas.setNewQuestionUI(currentQuestion);
    }

    int SelectContactPointIndex(int identifier)
    {
        for (int i = 0; i < contactPoints.points.Length; i++)
        {
            if (contactPoints.points[i].identifier == identifier)
            {
                return i;
            }
        }
        return -1;
    }

    //called on button animation finnished
    public void MoveToNextQuestion()
    {
        triviaCanvas.EnableEventSystem();
        if (!LoadNewQuestion())
        {
            //load end of trivia round canvas
            HideQuestionCanvas();
            triviaCanvas.ShowContiueAndRestartCanvas(questionCanvas.GetScore(), questions.Length);
        }
    }

    private void HideQuestionCanvas()
    {
        triviaCanvas.DisableQuestionCanvas();
    }

    private void EnableQuestionCanvas()
    {
        triviaCanvas.EnableQuestionCanvas();
    }

    //load the next question from the question pool
    private bool LoadNewQuestion()
    {
        if (currentQuestionNumber < questions.Length - 1)
        {
            currentQuestionNumber += 1;
            currentQuestion = questions[currentQuestionNumber];
            SetScoreText();
            questionCanvas.setNewQuestionUI(currentQuestion);
            return true;
        }
        else
        {
            return false;
        }

    }

    // for testing purposes only
    private QuestionData[] SetDummyRoundData()
    {

        QuestionData dummyQuestionData1 = new QuestionData();
        dummyQuestionData1.SetQuestionData(0, "What is the capital city of Finland", QuestionData.QuestionType.Textonly, ("Helsinki", true), ("Oulu", false), ("Kuopio", false), ("Kotka", false));
        QuestionData dummyQuestionData3 = new QuestionData();
        dummyQuestionData3.SetQuestionData(1, "Does Denmark belong to Nordic Countries", QuestionData.QuestionType.TrueFalse, ("FALSE", false), ("TRUE", true));
        QuestionData dummyQuestionData2 = new QuestionData();
        dummyQuestionData2.SetQuestionData(2, "Press the flag of Finland", QuestionData.QuestionType.Images, ("Finland", true), ("Sweden", false), ("Norway", false), ("Russia", false));
        QuestionData dummyQuestionData4 = new QuestionData();
        dummyQuestionData4.SetQuestionData(3, "Press the flag of Sweden", QuestionData.QuestionType.TrueFalseImage, ("Finland", false), ("Sweden", true));

        return new QuestionData[4] { dummyQuestionData1, dummyQuestionData2, dummyQuestionData3, dummyQuestionData4 };
    }
}
