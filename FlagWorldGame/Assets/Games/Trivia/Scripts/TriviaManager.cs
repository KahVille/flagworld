using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TriviaManager : MonoBehaviour
{

    ContactPointCollection contactPoints;
    private int currentContactPointIndex=1;

    [Header("Question Data")]

    //contains questions for current round. 
    [SerializeField]
    private QuestionData[] questions = null;
    private QuestionData currentQuestion = null;
    private int currentQuestionNumber = 0;

    [Header("Question UI")]

    [SerializeField]
    private Canvas contiueAndRestartCanvas = null;

    [SerializeField]
    private UIQuestionCanvas questionCanvas = null;

    [SerializeField]
    private GameObject eventSystem = null;

    [SerializeField]
    private GameObject loadingIndicator = null;

    [SerializeField]
    UIScoreText scoreText = null;

     private ModalPanel modalPanel;

    void Awake () {
        modalPanel = ModalPanel.Instance();
    }

    public void ShowPanel(string title = null, string desc=null) {
        EventButtonDetails button1Detail = new EventButtonDetails {buttonTitle = "Back to Title", action = ClosePanel};
        SpawnPanel(title,desc, button1Detail);
    }

    void SpawnPanel(string mainTitle, string titleDescription, EventButtonDetails button1, EventButtonDetails button2 = null, Sprite icon = null ) {
         ModalPanelDetails modalPanelDetails = new ModalPanelDetails {shortText = mainTitle,description = titleDescription, iconImage = icon};
          modalPanelDetails.button1Details = button1;
          modalPanelDetails.button2Details = button2;
          modalPanel.SpawnWithDetails (modalPanelDetails);
    }

    public void ClosePanel() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    IEnumerator Start()
    {
        loadingIndicator.SetActive(true);
        contactPoints = TriviaSaveLoadSystem.LoadContactPoints();
        if (contactPoints == null)
        { 
            loadingIndicator.SetActive(false);
            //spawnDialog where user needs to go back to menu
            ShowPanel("Error","contactPoint data is null");
        }
        else {
          loadingIndicator.SetActive(false);   
        //id that is inside the contact point;contactpoint.identifier
        currentContactPointIndex = SelectContactPointIndex(PlayerPrefs.GetInt("CurrentLocationIdentifier", 0));
        DisplayCurrentQuestionAndEnableCanvas();
        }

        yield return true;
    }

    private void DisplayCurrentQuestionAndEnableCanvas()
    {
        questions = contactPoints.points[currentContactPointIndex].questions;
        currentQuestion = questions[currentQuestionNumber];
        questionCanvas.gameObject.SetActive(true);
        scoreText.SetTextToDisplay($"{currentQuestionNumber +1 } / {questions.Length}");
        questionCanvas.setNewQuestionUI(currentQuestion);
    }

    int SelectContactPointIndex(int identifier) {
        for (int i = 0; i < contactPoints.points.Length; i++)
        {
           if(contactPoints.points[i].identifier == identifier) {
               return i;
           }
        }
        return -1;
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
            int roundScore = questionCanvas.GetComponent<UIQuestionCanvas>().GetScore();
            int numberOfQuestions = questions.Length;
            ShowContiueAndRestartCanvas(roundScore, numberOfQuestions);
        }
    }

    private void ShowContiueAndRestartCanvas(int roundScore, int numberOfQuestions)
    {
        contiueAndRestartCanvas.enabled = true;
        contiueAndRestartCanvas.GetComponent<UIContinueAndRestartCanvas>().SetTriviaScoreText(roundScore,numberOfQuestions);
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
            scoreText.SetTextToDisplay($"{currentQuestionNumber +1 } / {questions.Length}");
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
        dummyQuestionData1.SetQuestionData(0,"What is the capital city of Finland", QuestionData.QuestionType.Textonly, ("Helsinki", true), ("Oulu", false), ("Kuopio", false), ("Kotka", false));
        QuestionData dummyQuestionData3 = new QuestionData();
        dummyQuestionData3.SetQuestionData(1,"Does Denmark belong to Nordic Countries", QuestionData.QuestionType.TrueFalse, ("FALSE", false), ("TRUE", true));
        QuestionData dummyQuestionData2 = new QuestionData();
        dummyQuestionData2.SetQuestionData(2,"Press the flag of Finland", QuestionData.QuestionType.Images, ("Finland", true), ("Sweden", false), ("Norway", false), ("Russia", false));
        QuestionData dummyQuestionData4 = new QuestionData();
        dummyQuestionData4.SetQuestionData(3,"Press the flag of Sweden", QuestionData.QuestionType.TrueFalseImage, ("Finland", false), ("Sweden", true));

        return new QuestionData[4] { dummyQuestionData1, dummyQuestionData2, dummyQuestionData3, dummyQuestionData4 };
    }
}
