using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject gameOverPanel;
    public GameObject clearPanel;
    public GameObject pausePanel;
    public GameObject inGameUI;

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
        if (inGameUI != null) inGameUI.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (clearPanel != null) clearPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void ShowReadyScreen()
    {
        inGameUI.SetActive(false);
    }
}
