using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private Button pauseButton;

    void Start()
    {
        if (pauseButton != null)
        {

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