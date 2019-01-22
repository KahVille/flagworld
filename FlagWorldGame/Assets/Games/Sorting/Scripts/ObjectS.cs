using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectS : MonoBehaviour {

    private MeshRenderer MeshR;

    public int ShaderNum;
    public Texture[] Right;
    public Texture[] Left;
    public Texture[] Up;
    public Texture[] Down;

    private void Awake()
    {
        MeshR = GetComponent<MeshRenderer>();
        ShaderNum = Random.Range(0, 4);

        if (ShaderNum == 0) {
            int r = Random.Range(0, Right.Length);
            MeshR.material.mainTexture = Right[r];
        }
        else if(ShaderNum == 1)
        {
            int r = Random.Range(0, Left.Length);
            MeshR.material.mainTexture = Left[r];
        }
        else if(ShaderNum == 2)
        {
            int r = Random.Range(0, Up.Length);
            MeshR.material.mainTexture = Up[r];
        }
        else
        {
            int r = Random.Range(0, Down.Length);
            MeshR.material.mainTexture = Down[r];
        }
    }

}
