using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    private Button resumeButton;

    void Start()
    {
        if (resumeButton != null)
        {

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