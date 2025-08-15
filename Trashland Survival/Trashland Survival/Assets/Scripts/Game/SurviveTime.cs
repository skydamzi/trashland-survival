using UnityEngine;
using UnityEngine.UI;

public class SurviveTime : MonoBehaviour
{
    public Text surviveTimeText;

    void Awake()
    {
        surviveTimeText = GetComponent<Text>();
    }

    void Update()
    {
        if (GameManager.Instance != null && surviveTimeText != null)
        {
            float surviveTime = GameManager.Instance.gameTime;
            
            int minutes = Mathf.FloorToInt(surviveTime / 60);
            int seconds = Mathf.FloorToInt(surviveTime % 60);
            int milliseconds = Mathf.FloorToInt((surviveTime * 100) % 100);

            surviveTimeText.text = $"버틴 시간: {minutes:D2}:{seconds:D2}:{milliseconds:D2}";
        }
    }   
}
