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
    public static event Action OnGameExit; // 메인 메뉴로 돌아가기
    public static event Action OnUpgradeCardSelected; // 증강카드 선택 이벤트
    public static event Action OnTimeScaleRequestPause;  // 시간 조작 요청 (일시정지)
    public static event Action OnTimeScaleRequestResume; // 시간 조작 요청 (재개)
    public static event Action<GameState> OnGameStateChangeRequest; // 게임 상태 변경 요청

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
}
