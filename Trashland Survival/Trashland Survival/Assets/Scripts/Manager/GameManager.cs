using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public enum GameState
{
    Ready,
    Playing,
    Paused,
    GameOver,
    Clear
}

[System.Serializable]
public class PoolInfo
{
    public GameObject prefab;
    public int size;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;
    public float gameTime = 0f;
    public float highestTime = 0f;

    [Header("Object Pools")]
    public List<PoolInfo> poolsToPrepare;

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

    void PrepareObjectPools()
    {
        if (PoolManager.Instance == null) return;

        foreach (var pool in poolsToPrepare)
        {
            if (pool.prefab != null)
            {
                PoolManager.Instance.PreparePool(pool.prefab, pool.size);
            }
        }
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
            Time.timeScale = 0f;
        }
        else if (currentState == GameState.Playing)
        {
            GameEvents.GameResumed();
            Time.timeScale = 1f;
        }
        else if (currentState == GameState.Ready)
        {
            GameEvents.GameStateChanged(newState);
            Time.timeScale = 1f;
        }
    }

    public void StartGame()
    {
        GameEvents.NewGameStarted();
        PrepareObjectPools();
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

    public void ResetGameTime()
    {
        gameTime = 0f;
    }
}