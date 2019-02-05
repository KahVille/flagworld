using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int FlagPoints
    {
        get
        {
            return flagPoints;
        }
        set
        {
            flagPoints = value;
        }
    }
    int flagPoints = 0;
    
    public void AddScore(int newPoints = 1)
    {
        flagPoints += newPoints;
    }
}
