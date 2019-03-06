using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ContactPoint
{
    public int idetifier;
    public string name;
    public string description;
    public QuestionData[] questions;

    public ContactPoint(int id, string name, string desc, QuestionData[] questions) {
        this.idetifier = id;
        this.name = name;
        this.description = desc;
        this.questions = questions;
    }

}
