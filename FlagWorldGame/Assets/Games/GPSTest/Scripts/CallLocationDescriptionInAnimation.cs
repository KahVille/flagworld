using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLocationDescriptionInAnimation : MonoBehaviour
{
    public LocationDescription ldScript;
    public void CallLocationDesc()
    {
        ldScript.NeedCustomUpdate();
    }
}
