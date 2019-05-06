using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Location
{
    public string name;
    public double longitude;
    public double latitude;
    public double rangeDistance;
    public Image image;
    public string title;
    [TextArea]
    public string description;
}
