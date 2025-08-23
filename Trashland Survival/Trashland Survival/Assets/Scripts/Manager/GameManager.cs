using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    Ready,
    Playing,
    Paused,
    GameOver,
    Clear
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;
    public float gameTime = 0f;
    public float highestTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeState(GameState.Ready);
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            gameTime += Time.deltaTime;
        }
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log("게임 상태 변경: " + currentState);

        if (currentState == GameState.GameOver)
        {
            GameEvents.GameOver();
        }
        else if (currentState == GameState.Clear)
        {
            GameEvents.GameClear();
        }
        else if (currentState == GameState.Paused)
        {
            GameEvents.GamePaused();
            Time.timeScale = 0f; // 게임 일시 정지
        }
        else if (currentState == GameState.Playing)
        {
            GameEvents.GameResumed();
            Time.timeScale = 1f; // 게임 재개
        }
        else if (currentState == GameState.Ready)
        {
            GameEvents.GameStateChanged(newState);
            Time.timeScale = 1f; // 게임 준비 상태
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        SceneManager.LoadScene("inGame");
        Debug.Log("게임 시작");
        GameEvents.GameStarted();
    }
    public void PauseGame()
    {
        ChangeState(GameState.Paused);
        Debug.Log("게임 일시 정지");
        GameEvents.GamePaused();
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
        Debug.Log("게임 재개");
        GameEvents.GameResumed();
    }
    public void ExitGame()
    {
        if (gameTime > highestTime) highestTime = gameTime;
        ChangeState(GameState.Ready);
        SceneManager.LoadScene("MainMenu");
        gameTime = 0f;
        Debug.Log("게임 종료");
        GameEvents.GameExit();
    }
}