using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    public GameObject targetPanel;

    private Button navigationButton;

    void Start()
    {
        navigationButton = GetComponent<Button>();
        if (navigationButton != null)
        {
            navigationButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        if (targetPanel != null && UIManager.Instance != null)
        {
            UIManager.Instance.SwitchToPanel(targetPanel);
        }
    }
}
