using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button startButton;

    void Start()
    {
        if (startButton != null)
        {

            startButton.onClick.AddListener(GameManager.Instance.StartGame);
        }
        else
        {
            startButton = GetComponent<Button>();
            if (startButton != null)
            {
                startButton.onClick.AddListener(GameManager.Instance.StartGame);
            }
        }
    }
}