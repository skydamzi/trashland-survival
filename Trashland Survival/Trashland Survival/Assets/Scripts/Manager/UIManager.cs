using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject gameOverPanel;
    public GameObject clearPanel;
    public GameObject pausePanel;
    public GameObject inGameUI;
    public UpgradeUI upgradeUI; // 업그레이드 UI 참조
    public Text hpText;
    public Text expText;
    public Text dayStatusText;

    public AudioClip pauseSound;
    public AudioClip clickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        GameEvents.OnGameOver += ShowGameOverScreen;
        GameEvents.OnGameClear += ShowClearScreen;
        GameEvents.OnGamePaused += ShowPauseScreen;
        GameEvents.OnGameResumed += ShowInGameUI;
        GameEvents.OnGameExit += ShowReadyScreen;
        GameEvents.OnGameStateChanged += HandleGameStateChange;
    }

    void OnDisable()
    {
        GameEvents.OnGameOver -= ShowGameOverScreen;
        GameEvents.OnGameClear -= ShowClearScreen;
        GameEvents.OnGamePaused -= ShowPauseScreen;
        GameEvents.OnGameResumed -= ShowInGameUI;
        GameEvents.OnGameExit -= ShowReadyScreen;
        GameEvents.OnGameStateChanged -= HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState newState)
    {
        if (newState == GameState.Ready)
        {
            ShowReadyScreen();
        }
    }

    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnHpChanged += UpdateHpText;
            PlayerManager.Instance.OnExpChanged += UpdateExpText;
            PlayerManager.Instance.OnLevelUp += OnPlayerLevelUp;

            UpdateHpText();
            UpdateExpText();
        }
    }

    void UpdateHpText()
    {
        if (hpText != null && PlayerManager.Instance != null)
        {
            hpText.text = $"HP: {PlayerManager.Instance.currentHP} / {PlayerManager.Instance.maxHP}";
        }
    }

    void UpdateExpText()
    {
        if (expText != null && PlayerManager.Instance != null)
        {
            expText.text = $"EXP: {PlayerManager.Instance.currentExp} / {PlayerManager.Instance.maxExp}";
        }
    }

    void OnPlayerLevelUp()
    {
        Debug.Log("레벨업으로 증강 선택 UI 표시");
        if (upgradeUI != null)
        {
            var upgrades = UpgradeManager.Instance.GetRandomUpgrades(3);
            if (upgrades.Count > 0)
            {
                upgradeUI.ShowUpgrades(upgrades);
            }
            else
            {
                Debug.LogWarning("표시할 업그레이드 X");
            }
        }
    }

    void OnDestroy()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnHpChanged -= UpdateHpText;
            PlayerManager.Instance.OnExpChanged -= UpdateExpText;
            PlayerManager.Instance.OnLevelUp -= OnPlayerLevelUp;
        }
    }

    public void ShowGameOverScreen()
    {
        inGameUI.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void ShowClearScreen()
    {
        inGameUI.SetActive(false);
        clearPanel.SetActive(true);
    }

    public void ShowPauseScreen()
    {
        if (pauseSound != null)
        {
            SoundManager.Instance.PlaySFX(pauseSound);
        }
        inGameUI.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void HidePauseScreen()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        ShowInGameUI();
    }

    public void ShowInGameUI()
    {
        if (clickSound != null)
        {
            SoundManager.Instance.PlaySFX(clickSound);
        }
        if (inGameUI != null) inGameUI.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (clearPanel != null) clearPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void ShowReadyScreen()
    {
        if (clickSound != null)
        {
            SoundManager.Instance.PlaySFX(clickSound);
        }
        inGameUI.SetActive(false);
    }

    public void UpdateDayStatus(int dayNumber)
    {
        if (dayStatusText != null)
        {
            dayStatusText.text = $"DAY {dayNumber} 진행중";
        }
    }
}