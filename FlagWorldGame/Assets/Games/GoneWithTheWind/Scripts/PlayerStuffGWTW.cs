// ALSO CONTAINS FLAG-RELATED STUFFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStuffGWTW : MonoBehaviour
{
    Swipe swipe;
    public GameObject flag;
    public Transform topOfPole;
    public Transform bottomOfPole;
    public float smoothTime = 0.3f;
    Vector3 desiredPos;
    private Vector3 velocity = Vector3.zero;

    float flagHP;
    public float hpDecayMult;
    public Slider flagHPSlider;
    Material flagMat;
    float windTime;
    bool windy;
    bool gameOver;

    public GameObject gameOverPanel;
    public ParticleSystem windparticles;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        swipe = GetComponent<Swipe>();
        flagMat = flag.GetComponent<Renderer>().material;
        desiredPos = flag.transform.position;
        flagHP = 0f;
        windy = false;
        gameOver = false;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        StartCoroutine(WindRoutine());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit(); 
        }

        if(gameOver)
        {
            return;
        }

        if(windy && flag.transform.position.y > topOfPole.position.y - 1f)
        {
            if(flagMat.GetFloat("_WaveSpeed") < 100f)
            {
                flagMat.SetFloat("_WaveSpeed", 200f);
            }
            flagHP += Time.deltaTime * hpDecayMult;
            flagHPSlider.value = flagHP;
            if(flagHP >= 100f)
            {
                //GAME OVER
                gameOver = true;
                StartCoroutine(GameOverRoutine());
            }
        }

        if(flag.transform.position.y <= topOfPole.position.y - 1f)
        {
            flagMat.SetFloat("_WaveSpeed", 50f);
        }
    
        desiredPos.y -= (swipe.SwipeDelta.y * 0.01f);
        desiredPos.y = Mathf.Clamp(desiredPos.y, bottomOfPole.position.y, topOfPole.position.y);
        flag.transform.position = Vector3.SmoothDamp(flag.transform.position, desiredPos, ref velocity, smoothTime);
    }

    IEnumerator GameOverRoutine()
    {
        desiredPos.x += 10f;
        float timer = 0f;
        while(timer <= 1f)
        {
            flag.transform.position = Vector3.SmoothDamp(flag.transform.position, desiredPos, ref velocity, smoothTime);
            timer += Time.deltaTime / 1f;
            yield return null;
        }
        
        gameOverPanel.SetActive(true);
    }

    IEnumerator WindRoutine()
    {
        while(!gameOver)
        {
            // HEAVY WINDS!!
            windTime = Random.Range(1f, 4f);
            windy = true;
            windparticles.Play();
            yield return new WaitForSeconds(windTime);
            // CALM BEFORE THE STORM
            windparticles.Stop();
            flagMat.SetFloat("_WaveSpeed", 50f);
            windy = false;
            windTime = Random.Range(2f, 6f);
            yield return new WaitForSeconds(windTime);
        }
    }
}
