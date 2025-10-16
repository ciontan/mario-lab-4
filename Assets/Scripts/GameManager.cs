using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // Initialize events right away
    public UnityEvent gameStart = new UnityEvent();
    public UnityEvent gameRestart = new UnityEvent();
    public UnityEvent<int> scoreChange = new UnityEvent<int>();
    public UnityEvent gameOver = new UnityEvent();

    private int score = 0;
    private CoinAudioController coinAudioController;
    public IntVariable gameScore;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {

        gameStart.Invoke();
        Time.timeScale = 1.0f;
        SceneManager.activeSceneChanged += SceneSetup;
        // Get or add CoinAudioController
        coinAudioController = GetComponent<CoinAudioController>();
        if (coinAudioController == null)
        {
            coinAudioController = gameObject.AddComponent<CoinAudioController>();
            Debug.Log("Added CoinAudioController to GameManager");
        }
    }

    public void SceneSetup(Scene current, Scene next)
    {
        // Wait for next frame to ensure new scene is fully loaded
        StartCoroutine(SetupNewScene());
    }

    private IEnumerator SetupNewScene()
    {
        // Wait for the next frame to ensure new scene objects are initialized
        yield return null;

        // Find and setup the new HUD
        var hud = FindFirstObjectByType<HUDManager>();
        if (hud != null)
        {
            // Force a score update
            SetScore(score);
            // Trigger game start to setup UI positions
            gameStart.Invoke();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        gameScore.Value = 0;
        SetScore(score);

        // Reset coin audio
        if (coinAudioController != null)
            coinAudioController.ResetCoinCount();

        // Reset all QuestionBoxes
        foreach (var box in FindObjectsByType<QuestionBox>(FindObjectsSortMode.None))
        {
            box.ResetBox();
        }
        // Reset all BoxBricks
        foreach (var brick in FindObjectsByType<BoxBrick>(FindObjectsSortMode.None))
        {
            brick.ResetBox();
        }

        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    // Use this overload to specify if the score increase is from a coin
    public void IncreaseScore(int increment, bool isCoin = false)
    {
        Debug.Log("increasing score by " + increment);
        score += increment;
        gameScore.ApplyChange(1);

        // Play coin sound only if it's from a coin
        if (isCoin && coinAudioController != null)
        {
            coinAudioController.PlayCoinSound();
        }

        SetScore(score);
    }

    public void SetScore(int score)
    {
        //scoreChange.Invoke(gameScore.Value);
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        Debug.Log("gameover called");
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}