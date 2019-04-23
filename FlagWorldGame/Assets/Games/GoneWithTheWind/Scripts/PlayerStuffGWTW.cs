// ALSO CONTAINS FLAG-RELATED STUFFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerStuffGWTW : MonoBehaviour
{
    Swipe swipe;
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
    Material flagMat;
    float windTime;
    bool windy;
    bool gameOver;

    public GameObject gameOverPanel;
    public ParticleSystem windparticles;

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
        swipe = GetComponent<Swipe>();
        flagMat = flag.GetComponent<Renderer>().material;
        desiredPos = flag.transform.position;
        flagHP = 0f;
        windy = false;
        gameOver = false;
        score = 0;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        StartCoroutine(WindRoutine());
        StartCoroutine(BirdSpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SceneManager.LoadScene("MainMenu"); 
        }

        if(gameOver)
        {
            return;
        }

        score += Time.deltaTime * scoreMult * 15f;
        scoreMult = (flag.transform.position.y - bottomOfPole.position.y) / (topOfPole.position.y - bottomOfPole.position.y);
        scoreText.text = score.ToString("F2");

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
                StartGameOver();
            }
        }

        if(flag.transform.position.y <= topOfPole.position.y - 1f)
        {
            flagMat.SetFloat("_WaveSpeed", 50f);
        }
    
        desiredPos.y -= (swipe.SwipeDelta.y * 0.005f);
        desiredPos.y = Mathf.Clamp(desiredPos.y, bottomOfPole.position.y, topOfPole.position.y);
        flag.transform.position = Vector3.SmoothDamp(flag.transform.position, desiredPos, ref velocity, smoothTime);
        debugText.text = swipe.SwipeDelta.y.ToString();
    }

    public void StartGameOver()
    {
        gameOver = true;
        StartCoroutine(GameOverRoutine());
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
            windparticles.Play();
            yield return new WaitForSeconds(1.5f);
            windTime = Random.Range(1f, 4f);
            windy = true;
            yield return new WaitForSeconds(windTime);
            // CALM BEFORE THE STORM
            windparticles.Stop();
            yield return new WaitForSeconds(0.5f);
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
        spawnPos.x = -7f;
        while(!gameOver)
        {
            birdTime = Random.Range(1f,10f);
            yield return new WaitForSeconds(birdTime);
            spawnPos.y = Random.Range(bottomOfPole.position.y, topOfPole.position.y - 1f);
            Vector3 spawnPosInVP = Camera.main.WorldToViewportPoint(spawnPos);
            spawnPosInVP.x = 0.15f;
            spawnPosInVP = Camera.main.ViewportToWorldPoint(spawnPosInVP);
            spawnPosInVP.z = -2f;
            birdWarning.transform.position = spawnPosInVP;
            birdWarning.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            birdWarning.SetActive(false);
            GameObject newBird = Instantiate(bird, spawnPos, Quaternion.identity);
            
            //newBird.GetComponent<Rigidbody2D>().velocity = Vector2.right * 5f;
            newBird.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5f * 70f);
            Destroy(newBird, 5f);
        }
    }
}
