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
            gameObject.GetComponent<Image>().sprite = imageSprite;
        answerText.SetText($"{answer.answerText}");
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
