using UnityEngine;
using UnityEngine.SceneManagement;

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
            UIManager.Instance.ShowGameOverScreen();
        }
        else if (currentState == GameState.Clear)
        {
            UIManager.Instance.ShowClearScreen();
        }
        else if (currentState == GameState.Paused)
        {
            UIManager.Instance.ShowPauseScreen();
            Time.timeScale = 0f; // 게임 일시 정지
        }
        else if (currentState == GameState.Playing)
        {
            UIManager.Instance.ShowInGameUI();
            Time.timeScale = 1f; // 게임 재개
        }
        else if (currentState == GameState.Ready)
        {
            UIManager.Instance.ShowReadyScreen();
            Time.timeScale = 1f; // 게임 준비 상태
        }
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        SceneManager.LoadScene("inGame");
        Debug.Log("게임 시작");
    }
    public void PauseGame()
    {
        ChangeState(GameState.Paused);
        Debug.Log("게임 일시 정지");
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
        Debug.Log("게임 재개");
    }
    public void ExitGame()
    {
        if (gameTime > highestTime) highestTime = gameTime;
        ChangeState(GameState.Ready);
        SceneManager.LoadScene("MainMenu");
        gameTime = 0f;
        Debug.Log("게임 종료");
    }
}