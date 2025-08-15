using UnityEngine;
using UnityEngine.UI;

public class HighestTime : MonoBehaviour
{
    public Text highestTimeText;

    void Awake()
    {
        highestTimeText = GetComponent<Text>();
    }

    void Update()
    {
        if (GameManager.Instance != null && highestTimeText != null)
        {
            float highestTime = GameManager.Instance.highestTime;

            int minutes = Mathf.FloorToInt(highestTime / 60);
            int seconds = Mathf.FloorToInt(highestTime % 60);
            int milliseconds = Mathf.FloorToInt((highestTime * 100) % 100);

            highestTimeText.text = $"최고 기록: {minutes:D2}:{seconds:D2}:{milliseconds:D2}";
        }
    }   
}
