using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System;

public class UIManager : MonoBehaviour, PlayerObserver
{

    
    [SerializeField]  GameObject mainMenu;
    [SerializeField]  GameObject store;
    [SerializeField]  GameObject config;
    [SerializeField]  GameObject pause;
    [SerializeField]  GameObject gameplay;
    [SerializeField] GameObject lost;
    [SerializeField] GameObject road;
    [SerializeField] RoadSpawner roadVelocity;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreText2;
    [SerializeField] TextMeshProUGUI totalCarrotsText;
    private int score = 0;
    private int highscore = 0;
    private int totalCarrots = 100;
    [Header("Env")]
    [SerializeField]  float transitionDuration ;
    [SerializeField] Camera Camera;

    [SerializeField] PlayerSubject playerSubject;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float transitionTimer = 0.0f;
    private bool isTransitioning = false;
    private bool isMainMenu = true;
    private static UIManager instance;

    public Action<GAME_STATE> OnGameStateChanged;
    GAME_STATE currentGameState = GAME_STATE.GAMEPLAY;

    private void Awake()
    {
        instance = this;
    }
    public void OnNotify(PlayerEnum playerActions)
    {
        switch (playerActions)
        {
            case(PlayerEnum.CollectCarrots):
                AddPoint();
                break;
            case(PlayerEnum.Die):
                Loose();
                break;
        }
    }
    public static UIManager GetInstance()
    {
        return instance;
    }

    private void OnEnable()
    {
        playerSubject.AddObserver(this);
    }
    private void OnDisable()
    {
        playerSubject.RemoveObserver(this);
    }

    public void GameStateChange(GAME_STATE _state)
    {
        currentGameState = _state;
        if (OnGameStateChanged != null)
        {
            OnGameStateChanged(currentGameState);
        }
        Debug.Log(currentGameState);
    }
    void Start()
    {
        GameStateChange(GAME_STATE.MAINMENU);
        initialPosition = Camera.transform.position;
        initialRotation = Camera.transform.rotation;
        road.SetActive(false);
        ChangeScore();
        totalCarrots = PlayerPrefs.GetInt("totalCarrots", 100);
        totalCarrotsText.text = "Total Carrots: " + totalCarrots.ToString();
        Debug.Log(totalCarrots);
    }


    private void AddPoint()
    {
        score += 1000;
        scoreText.text = "Score: " + score.ToString();
        scoreText2.text = "Score: " + score.ToString();
        if (highscore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
            highscore = PlayerPrefs.GetInt("highscore", 0);
            highscoreText.text = "Highscore: " + highscore.ToString();
        }
    }


    private void TotalCarrots()
    {
       
        totalCarrots += score;
        PlayerPrefs.SetInt("totalCarrots", totalCarrots);
        totalCarrotsText.text = "Total Carrots: " + totalCarrots.ToString();
    }

    public int GetTotalCarrots()
    {
        return totalCarrots;
    }

    public void SpendCarrots(int amount)
    {
        totalCarrots -= amount;
        PlayerPrefs.SetInt("totalCarrots", totalCarrots);
        totalCarrotsText.text = "Total Carrots: " + totalCarrots.ToString();
    }



    private void ChangeScore()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = "Score: " + score.ToString();
        scoreText2.text = "Score: " + score.ToString();
        highscoreText.text = "Highscore: " + highscore.ToString();
    }
    private void Loose()
    {
        gameplay.SetActive(false);
        lost.SetActive(true);
        roadVelocity.roadSpeed = 0;
        GameStateChange(GAME_STATE.GAMEOVER);


    }
    public void OpenStore()
    {
        if (isTransitioning) return;

        isMainMenu = false;
        mainMenu.SetActive(false);



        targetPosition = new Vector3(-8.344361f, 5.722942f, 10.39388f);
        targetRotation = Quaternion.Euler(20, 184, 0);

        transitionTimer = 0.0f;
        isTransitioning = true;
    }
    public void OpenConfig()
    {
        mainMenu.SetActive(false);
        config.SetActive(true);
    }
    public void BackConfig()
    {
        mainMenu.SetActive(true);
        config.SetActive(false);

    }
    public void StartPlaying()
    {
        mainMenu.SetActive(false );
        gameplay.SetActive(true);
        score = 0;
        ChangeScore();
        road.SetActive(true);
        GameStateChange(GAME_STATE.GAMEPLAY);

    }

    public void Pause() 
    {
        gameplay.SetActive(false);
        pause.SetActive(true);
        roadVelocity.roadSpeed = 0;
        GameStateChange(GAME_STATE.PAUSE);




    }
    public void QuitPause()
    {
        gameplay.SetActive(true);
        pause.SetActive(false);
        GameStateChange(GAME_STATE.GAMEPLAY);

        roadVelocity.roadSpeed = 5;


    }

    public void ReturnToMainMenu()
    {
        if (isTransitioning) return;

        isMainMenu = true;
        store.SetActive(false);



        targetPosition = initialPosition; 
        targetRotation = initialRotation;

        transitionTimer = 0.0f;
        isTransitioning = true;
        print(isMainMenu);
        road.SetActive(false);
        GameStateChange(GAME_STATE.MAINMENU);


    }
    public void ExitPlay()
    {
      
        TotalCarrots();
        road.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void Restart()
    {
        TotalCarrots();
        road.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void DeleteInformation()
    {
        PlayerPrefs.DeleteAll();
        totalCarrots = PlayerPrefs.GetInt("totalCarrots", 0);
        totalCarrotsText.text = "Total Carrots: " + totalCarrots.ToString();

        score = 0;
      ChangeScore() ;
       
    }



    void Update()
    {
        if (isTransitioning)
        {

            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            if (isMainMenu)
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, initialPosition, t); 
                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, initialRotation, t);
                if (Vector3.Distance(Camera.transform.position, initialPosition) < 0.01f &&
       Quaternion.Angle(Camera.transform.rotation, initialRotation) < 0.1f)
                {
                    t = 1;
                    mainMenu.SetActive(true);
                }
            }
            else
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, targetPosition, t);
                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, targetRotation, t);
                if (Vector3.Distance(Camera.transform.position, targetPosition) < 0.01f &&
        Quaternion.Angle(Camera.transform.rotation, targetRotation) < 0.1f)
                {
                    t = 1;
                    store.SetActive(true);
                }
            }

            if (t >= 1f)
            {
                isTransitioning = false;
            }
        }
    }

 
}
public enum GAME_STATE
{
    MAINMENU,
    GAMEPLAY,
    PAUSE,
    GAMEOVER,
    RESTART,

}