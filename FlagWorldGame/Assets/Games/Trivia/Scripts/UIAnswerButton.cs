using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIAnswerButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI answerText = null;
    [SerializeField]
    private Sprite defaultSprite = null;
    Animator animator = null;
    TriviaManager triviaManager = null;

    void Start()
    {
        animator = GetComponent<Animator>();
        triviaManager = (TriviaManager) FindObjectOfType(typeof(TriviaManager));
    }

    public void OnQuestionChange(AnswerData answer = null, Sprite imageSprite = null)
    {
        gameObject.SetActive(true);
        if (imageSprite != null)
        {
            gameObject.GetComponent<Image>().sprite = imageSprite;
            answerText.SetText("");
            GetComponent<RectTransform>().sizeDelta = new Vector2(500, 250);
        }
        else
        {
            answerText.SetText($"{answer.answerText}");
            GetComponent<RectTransform>().sizeDelta = new Vector2(800, 250);
        }
    }

    public void ClearButtonData()
    {
        gameObject.GetComponent<Image>().sprite = defaultSprite;
        gameObject.SetActive(false);
    }

    public void SetCorrectAnswerColor()
    {
        Debug.Log("Play correct animation");
        animator.SetTrigger("CorrectAnswer");
    }

    public void SetWrongAnswerColor()
    {
         animator.SetTrigger("WrongAnswer");
    }

    // called from Animator event
    public void OnAnimationFinished() {
        Debug.Log("animation finnished");
        triviaManager.MoveToNextQuestion();
    }

}
