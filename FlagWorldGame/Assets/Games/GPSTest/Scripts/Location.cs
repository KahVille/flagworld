// Class for the location.

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
    public string identifier;
    [TextArea]
    public string description;
}
