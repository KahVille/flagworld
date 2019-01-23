using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingArea : MonoBehaviour
{
    private LajitteluSpawner LS;

    private void Awake()
    {
        LS = GameObject.Find("Spawner").GetComponent<LajitteluSpawner>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Mathf.Abs(collision.transform.position.x) >= GameObject.Find("Background").transform.localScale.x / 2)
        {
            if (collision.transform.position.x > 0)
            {
                if (collision.GetComponent<ObjectS>().ShaderNum == 0)
                {
                    LS.RightDir();
                }
                else
                {
                    LS.WrongDir();
                }
            }
            else
            {
                if (collision.GetComponent<ObjectS>().ShaderNum == 1)
                {
                    LS.RightDir();
                }
                else
                {
                    LS.WrongDir();
                }
            }
        }
        else
        {
            if (collision.transform.position.y > 0)
            {
                if (collision.GetComponent<ObjectS>().ShaderNum == 2)
                {
                    LS.RightDir();
                }
                else
                {
                    LS.WrongDir();
                }
            }
            else
            {
                if (collision.GetComponent<ObjectS>().ShaderNum == 3)
                {
                    LS.RightDir();
                }
                else
                {
                    LS.WrongDir();
                }
            }
        }
        Destroy(collision.gameObject);
        LS.SetSpawn();
    }
}
