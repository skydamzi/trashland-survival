using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Button pauseButton;
    public AudioClip pauseSound;

    void Start()
    {
        if (pauseButton != null)
        {
            SoundManager.Instance.PlaySFX(pauseSound);
            pauseButton.onClick.AddListener(GameManager.Instance.PauseGame);
        }
        else
        {
            pauseButton = GetComponent<Button>();
            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(GameManager.Instance.PauseGame);
            }
        }
    }
}