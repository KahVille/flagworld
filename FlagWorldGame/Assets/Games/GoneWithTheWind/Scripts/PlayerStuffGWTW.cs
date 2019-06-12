// Contains input during the Gone With The Wind-game. Is also in charge
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
    public float smoothTime;
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
    public BoxCollider2D flagCol;
    bool gameOver;
    bool isWin;
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
    public Canvas startCanvas;
    public Canvas victoryCanvas;
    public TextMeshProUGUI victoryPointsTxt;
    public Canvas loseCanvas;
    public TextMeshProUGUI losePointsTxt;

    //DEBUG
    public TextMeshProUGUI debugText;

    void Start()
    {
        // gameOver is set to true in the beginning so the game doesnt run when the start canvas is on.
        // It is turned off when the player clicks the Start button on the start canvas and the game begins.
        gameOver = true;
        if(!PlayerPrefs.HasKey("GWTWHigh"))
        {
            PlayerPrefs.SetFloat("GWTWHigh", 9999.0f);
        }
        if(PlayerPrefs.GetInt("CurrentGameHigh") == 1) {
            CallReStart();
        }
    }

    public void CallReStart()
    {
        StartCoroutine(ReStart());
    }

    // Start is called before the first frame update
    public IEnumerator ReStart()
    {
        mainCam = Camera.main;
        flagMat = flag.GetComponent<Renderer>().material;
        desiredPos = flag.transform.position;
        flagHP = 0f;
        windy = false;
        gameOver = false;
        isWin = false;
        score = 0;
        windMat = windparticles.GetComponent<ParticleSystemRenderer>().material;
        windCol.enabled = false;
        startCanvas.enabled = false;
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

        //score += Time.deltaTime * scoreMult * 15f;
        //scoreMult = (flag.transform.position.y - bottomOfPole.position.y) / (topOfPole.position.y - bottomOfPole.position.y);
        score += Time.deltaTime;
        scoreText.text = score.ToString("F2");

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
        desiredPos.y += mouseScroll * 5.0f;
    }

    public void StartGameOver()
    {
        gameOver = true;
        StartCoroutine(GameOverRoutine());
    }

    public void Damage(float howMuch)
    {
        if(gameOver || isWin)
        {
            return;
        }
        flagHP += howMuch;
        flagHPSlider.value = flagHP;
        if(flagHP >= 100f)
        {
            mainCam.GetComponent<CameraMovementGWTW>().enabled = false;
            flagCol.enabled = false;
            //GAME OVER
            StartGameOver();
        }
    }

    public void TimeDamage(float howMuch)
    {
        score += howMuch;
    }

    void Victory()
    {
        victoryCanvas.enabled = true;
        isWin = true;
        //score += 50.0f;
        string localized_time = (LocalizationManager.Instance != null) ?
                                $"{LocalizationManager.Instance.GetLocalizedValue("time_text")} {score.ToString()} \n"
                                : $"Time {score.ToString()} \n";
        victoryPointsTxt.text = localized_time;
        if((score < PlayerPrefs.GetFloat("GWTWHigh")) || (PlayerPrefs.GetFloat("GWTWHigh") == 0))
        {
            string localized_nNewBestTime = (LocalizationManager.Instance != null) ?
                                $"{LocalizationManager.Instance.GetLocalizedValue("new_best_time")}"
                                : $"New best time!";
            victoryPointsTxt.text += localized_nNewBestTime;
            PlayerPrefs.SetFloat("GWTWHigh", score);
        }
        else
        {
            string localized_BestTime = (LocalizationManager.Instance != null) ?
                                $"{LocalizationManager.Instance.GetLocalizedValue("best_time")} {PlayerPrefs.GetFloat("GWTWHigh").ToString()}"
                                : $"best time!";
            victoryPointsTxt.text += localized_BestTime;
        }
        //victoryPanel.SetActive(true);
        //scorePanelAnim.SetBool("Move", true);
    }

    IEnumerator GameOverRoutine()
    {
        desiredPos.x += 25f;
        float timer = 0f;
        while(timer <= 1f)
        {
            flag.transform.position = Vector3.SmoothDamp(flag.transform.position, desiredPos, ref velocity, smoothTime);
            timer += Time.deltaTime / 1f;
            yield return null;
        }
        
        //gameOverPanel.SetActive(true);
        loseCanvas.enabled = true;
        string localized_time = (LocalizationManager.Instance != null) ?
                                $"{LocalizationManager.Instance.GetLocalizedValue("time_text")} {score.ToString()} \n"
                                : $"Time {score.ToString()} \n";
        losePointsTxt.text = localized_time;
        // if(score > PlayerPrefs.GetFloat("GWTWHigh"))
        // {
        //     losePointsTxt.text += "\nNew Highscore!";
        //     PlayerPrefs.SetFloat("GWTWHigh", score);
        // }
        // else
        // {
        //     losePointsTxt.text += "\nHighscore: " + PlayerPrefs.GetFloat("GWTWHigh").ToString();
        // }
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
            windSpawnPos.y += Random.Range(3f, 12f);
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
            yield return new WaitForSeconds(1.4f);
            flagMat.SetFloat("_WaveSpeed", 50f);
            windCol.enabled = false;
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
            birdTime = Random.Range(0.7f, 1.5f);
            yield return new WaitForSeconds(birdTime);
            spawnPos.y = flag.transform.position.y + Random.Range(5f, 20f);
            Vector3 spawnPosInVP = Camera.main.WorldToViewportPoint(spawnPos);
            // spawn left or right
            bool goRight = Random.Range(0, 2) == 0 ? true : false;
            if(goRight)
            {
                spawnPosInVP.x = 0.2f;
                spawnPosInVP = Camera.main.ViewportToWorldPoint(spawnPosInVP);
                spawnPosInVP.z = -2f;
                birdWarning.transform.position = spawnPosInVP;
                birdWarning.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                birdWarning.SetActive(false);
                spawnPos.x = -10f;
                GameObject newBird = Instantiate(bird, spawnPos, Quaternion.identity);
                float randSpeed = Random.Range(0.7f, 1.8f);
                newBird.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5f * 70f * randSpeed);
                Destroy(newBird, 7f);
            }
            else
            {
                spawnPosInVP.x = 0.8f;
                spawnPosInVP = Camera.main.ViewportToWorldPoint(spawnPosInVP);
                spawnPosInVP.z = -2f;
                birdWarning.transform.position = spawnPosInVP;
                birdWarning.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                birdWarning.SetActive(false);
                spawnPos.x = 10f;
                GameObject newBird = Instantiate(bird, spawnPos, Quaternion.identity);
                newBird.transform.localScale = new Vector3(
                    -newBird.transform.localScale.x,
                    newBird.transform.localScale.y,
                    newBird.transform.localScale.z
                    );
                float randSpeed = Random.Range(0.7f, 1.8f);
                newBird.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 5f * 70f * randSpeed);
                Destroy(newBird, 7f);
            }
        }
    }
}
