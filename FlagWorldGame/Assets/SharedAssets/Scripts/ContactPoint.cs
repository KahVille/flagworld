using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ContactPoint
{
    public int identifier;
    public string name;
    public string description;
    public QuestionData[] questions;

    public ContactPoint(int identifier, string name, string desc, QuestionData[] questions) {
        this.identifier = identifier;
        this.name = name;
        this.description = desc;
        this.questions = questions;
    }

}
