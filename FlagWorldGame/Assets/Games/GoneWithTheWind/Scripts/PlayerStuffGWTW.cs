﻿// Contains input during the Gone With The Wind-game. Is also in charge
// of flags HP and constricting the flag's movement. 

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerStuffGWTW : MonoBehaviour
{
    Camera mainCam;
    Vector3 touchStartPos;
    Vector3 touchDirection;
    public float scrollSpeed;
    public GameObject flag;
    public Transform topOfPole;
    public Transform bottomOfPole;
    public float smoothTime = 0.3f;
    Vector3 desiredPos;
    private Vector3 velocity = Vector3.zero;

    public float FlagHP
    {
        get
        {
            return flagHP;
        }
        set
        {
            flagHP = value;
        }
    }
    float flagHP;
    public float hpDecayMult;
    public Slider flagHPSlider;
    public Material flagMat;
    float windTime;
    public bool Windy
    {
        get
        {
            return windy;
        }
        set
        {
            windy = value;
        }
    }
    bool windy;
    public BoxCollider2D windCol;
    bool gameOver;

    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public Animator scorePanelAnim;
    public ParticleSystem windparticles;
    public GameObject windObj;
    Material windMat;
    float score;
    float scoreMult;
    public TextMeshProUGUI scoreText;

    public GameObject bird;
    public GameObject birdWarning;

    //DEBUG
    public TextMeshProUGUI debugText;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        mainCam = Camera.main;
        flagMat = flag.GetComponent<Renderer>().material;
        desiredPos = flag.transform.position;
        flagHP = 0f;
        windy = false;
        gameOver = false;
        score = 0;
        windMat = windparticles.GetComponent<ParticleSystemRenderer>().material;
        windCol.enabled = false;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        StartCoroutine(WindRoutine());
        StartCoroutine(BirdSpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            return;
        }

        if(flag.transform.position.y >= topOfPole.position.y - 2f)
        {
            gameOver = true;
            Victory();
        }

        score += Time.deltaTime * scoreMult * 15f;
        scoreMult = (flag.transform.position.y - bottomOfPole.position.y) / (topOfPole.position.y - bottomOfPole.position.y);
        scoreText.text = score.ToString("F2");

        // Move this to flagCollision
        if(windy)
        {
            if(flagMat.GetFloat("_WaveSpeed") < 100f)
            {
                flagMat.SetFloat("_WaveSpeed", 300f);
            }
            Damage(Time.deltaTime * hpDecayMult);
        }

        if(flag.transform.position.y <= topOfPole.position.y - 1f && !windy)
        {
            flagMat.SetFloat("_WaveSpeed", 50f);
        }

        #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        TouchInput();
        #elif UNITY_EDITOR
        DebugInput();
        #endif

        desiredPos.y = Mathf.Clamp(desiredPos.y, bottomOfPole.position.y, topOfPole.position.y);
        flag.transform.position = Vector3.SmoothDamp(flag.transform.position, desiredPos, ref velocity, smoothTime);
    }

    void TouchInput()
    {
        // Move the flag with 1 finger
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                touchStartPos = mainCam.ScreenToWorldPoint(touch.position);
            }

            touchDirection = touchStartPos - mainCam.ScreenToWorldPoint(touch.position);
            desiredPos.y += touchDirection.y * scrollSpeed;
            desiredPos.z = 1.5f;
        }
    }

    // Mouse movement for debug
    void DebugInput()
    {
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        desiredPos.y += mouseScroll;
    }

    public void StartGameOver()
    {
        gameOver = true;
        StartCoroutine(GameOverRoutine());
    }

    public void Damage(float howMuch)
    {
        flagHP += howMuch;
        flagHPSlider.value = flagHP;
        if(flagHP >= 100f)
        {
            mainCam.GetComponent<CameraMovementGWTW>().enabled = false;
            //GAME OVER
            StartGameOver();
        }
    }

    void Victory()
    {
        victoryPanel.SetActive(true);
        scorePanelAnim.SetBool("Move", true);
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

    IEnumerator FadeWind()
    {
        float lerp = 0;
        Color startCol = Color.white;
        Color endCol = Color.clear;
        ParticleSystem.MainModule mm = windparticles.main;
        while(lerp <= 1f)
        {
            mm.startColor = Color.Lerp(startCol, endCol, lerp);
            lerp += Time.deltaTime / 0.8f;
            yield return null;
        }
        windparticles.Clear();
    }

    IEnumerator WindRoutine()
    {
        Vector3 windSpawnPos;
        while(!gameOver)
        {
            // HEAVY WINDS!!
            windparticles.Play();
            ParticleSystem.MainModule mm = windparticles.main;
            mm.startColor = Color.white;
            windSpawnPos = flag.transform.position;
            windSpawnPos.y += Random.Range(2f, 15f);
            windSpawnPos.x -= 11f;
            windObj.transform.position = windSpawnPos;
            yield return new WaitForSeconds(1.5f);
            windCol.enabled = true;
            //windTime = Random.Range(1f, 4f);
            yield return new WaitForSeconds(3.5f);
            // CALM BEFORE THE STORM
            windparticles.Stop();
            //StartCoroutine(FadeWind());
            //windparticles.Clear();
            windCol.enabled = false;
            yield return new WaitForSeconds(0.8f);
            flagMat.SetFloat("_WaveSpeed", 50f);
            windy = false;
            windTime = Random.Range(2f, 6f);
            yield return new WaitForSeconds(windTime);
        }
    }

    IEnumerator BirdSpawnRoutine()
    {
        float birdTime;
        Vector3 spawnPos;
        spawnPos = Vector3.zero;
        spawnPos.z = -2f;
        spawnPos.x = -10f;
        while(!gameOver)
        {
            birdTime = Random.Range(1f,10f);
            yield return new WaitForSeconds(birdTime);
            spawnPos.y = flag.transform.position.y + Random.Range(-3f, 15f);
            Vector3 spawnPosInVP = Camera.main.WorldToViewportPoint(spawnPos);
            spawnPosInVP.x = 0.2f;
            spawnPosInVP = Camera.main.ViewportToWorldPoint(spawnPosInVP);
            spawnPosInVP.z = -2f;
            birdWarning.transform.position = spawnPosInVP;
            birdWarning.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            birdWarning.SetActive(false);
            GameObject newBird = Instantiate(bird, spawnPos, Quaternion.identity);
            
            //newBird.GetComponent<Rigidbody2D>().velocity = Vector2.right * 5f;
            newBird.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5f * 70f);
            Destroy(newBird, 7f);
        }
    }
}
