using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIAnswerText : MonoBehaviour
{
    TextMeshProUGUI answerText;
    [SerializeField]
    private Sprite defaultSprite = null;
    void Start()
    {
        answerText = GetComponent<TextMeshProUGUI>();
    }

    public void OnQuestionChange(AnswerData answer = null, Sprite imageSprite = null) {
        gameObject.transform.parent.gameObject.SetActive(true);
        if(imageSprite!=null);
            gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = imageSprite;
        answerText.SetText($"{answer.answerText}");
    }

    public void ClearButtonData() {
        gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = defaultSprite;
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
