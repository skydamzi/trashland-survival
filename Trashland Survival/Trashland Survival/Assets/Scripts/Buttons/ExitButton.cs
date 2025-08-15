using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button exitButton;

    void Start()
    {
        if (exitButton != null)
        {

            exitButton.onClick.AddListener(GameManager.Instance.ExitGame);
        }
        else
        {
            exitButton = GetComponent<Button>();
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(GameManager.Instance.ExitGame);
            }
        }
    }
}