using System;

public static class GameEvents
{
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;
    public static event Action OnGameClear;
    public static event Action OnGameExit;

    public static void GameStateChanged(GameState newState)
    {
        OnGameStateChanged?.Invoke(newState);
    }

    public static void GameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public static void GamePaused()
    {
        OnGamePaused?.Invoke();
    }

    public static void GameResumed()
    {
        OnGameResumed?.Invoke();
    }

    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void GameClear()
    {
        OnGameClear?.Invoke();
    }

    public static void GameExit()
    {
        OnGameExit?.Invoke();
    }
}
