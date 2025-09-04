using System;
using UnityEngine.Events;

public static class GameEvents
{
    public static event Action OnNewGameStarted;
    public static UnityEvent OnWeaponSwapRequested = new UnityEvent();
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;
    public static event Action OnGameOver;
    public static event Action OnGameClear;
    public static event Action OnGameExit;
    public static event Action OnUpgradeCardSelected;
    public static event Action OnTimeScaleRequestPause;
    public static event Action OnTimeScaleRequestResume;
    public static event Action<GameState> OnGameStateChangeRequest;
    public static event Action<WeaponData> WeaponEnableRequested;

    public static void NewGameStarted()
    {
        OnNewGameStarted?.Invoke();
    }

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

    public static void UpgradeCardSelected()
    {
        OnUpgradeCardSelected?.Invoke();
    }

    public static void RequestTimeScalePause()
    {
        OnTimeScaleRequestPause?.Invoke();
    }

    public static void RequestTimeScaleResume()
    {
        OnTimeScaleRequestResume?.Invoke();
    }

    public static void RequestGameStateChange(GameState newState)
    {
        OnGameStateChangeRequest?.Invoke(newState);
    }
    
    public static void OnWeaponEnableRequested(WeaponData weaponData)
    {
        WeaponEnableRequested?.Invoke(weaponData);
    }

}
