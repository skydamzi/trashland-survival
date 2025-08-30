using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    private Button resumeButton;
    public AudioClip resumeSound;

    void Start()
    {
        if (resumeButton != null)
        {
            SoundManager.Instance.PlaySFX(resumeSound);
            resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);
        }
        else
        {
            resumeButton = GetComponent<Button>();
            if (resumeButton != null)
            {
                resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);
            }
        }
    }
}