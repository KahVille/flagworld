using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIQuestionText : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI questionText;

    // Start is called before the first frame update

    public void SetQuestionText(string textToSet = "") {
        questionText.SetText($"{textToSet}");
    }


}
