using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

// public class HUDManager : Singleton<HUDManager>
public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-747, 473, 0),
        new Vector3(0, 0, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(844, 455, 0),
        new Vector3(0, -150, 0)
    };

    public GameObject scoreText;
    public GameObject restartButtonObj;
    public GameObject pauseButtonObj;

    public GameObject gameOverPanel;
    // public override void Awake()
    
    public GameObject highscoreText;
    public IntVariable gameScore;
    void Awake()
    {
        // base.Awake();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Start()
    {
        // Make sure GameManager exists and subscribe to its events
        if (GameManager.instance != null)
        {
            GameManager.instance.gameStart.AddListener(GameStart);
            GameManager.instance.gameOver.AddListener(GameOver);
            GameManager.instance.gameRestart.AddListener(GameStart);
            GameManager.instance.scoreChange.AddListener(SetScore);
            GameStart(); // Set initial state
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);

        // Force the UI elements back to their original positions with ForceLayoutImmediateRecursively
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButtonObj.transform.localPosition = restartButtonPosition[0];
        if (restartButtonObj != null) restartButtonObj.SetActive(true);
        if (pauseButtonObj != null) pauseButtonObj.SetActive(true);

        // Force immediate layout update to ensure positions are applied
        Canvas.ForceUpdateCanvases();

        // Log positions for debugging
        Debug.Log("GameStart: Score position set to " + scoreText.transform.localPosition);
        Debug.Log("GameStart: Button position set to " + restartButtonObj.transform.localPosition);

        // Make sure time scale is running
        Time.timeScale = 1.0f;
        Debug.Log("HUDManager GameStart called, set Time.timeScale = " + Time.timeScale);
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButtonObj.transform.localPosition = restartButtonPosition[1];
        if (restartButtonObj != null) restartButtonObj.SetActive(false);
        if (pauseButtonObj != null) pauseButtonObj.SetActive(false);
        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }
}
