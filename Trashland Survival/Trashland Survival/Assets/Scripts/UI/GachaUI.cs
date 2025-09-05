using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    public GachaPoolData gachaPool;
    public Text goldText;
    public Text costText;
    public Button gachaButton;
    public Text resultText;

    void Start()
    {
        if (gachaButton != null)
        {
            gachaButton.onClick.AddListener(OnGachaButtonClicked);
        }
        RefreshUI();
    }

    void Update()
    {
        RefreshUI(); // 나중에 이벤트 기반으로 변경하기
    }

    void RefreshUI()
    {
        if (PlayerManager.Instance != null && goldText != null)
        {
            goldText.text = $"골드: {PlayerManager.Instance.gold}";
        }
        if (gachaPool != null && costText != null)
        {
            costText.text = $"비용: {gachaPool.costGold} G";
        }
    }

    void OnGachaButtonClicked()
    {
        if (GachaManager.Instance != null && gachaPool != null)
        {
            EquipmentData result = GachaManager.Instance.PerformGacha(gachaPool);
            if (result != null)
            {
                if (resultText != null)
                {
                    resultText.text = $"획득: {result.itemName}";
                }
            }
            else
            {
                if (resultText != null)
                {
                    resultText.text = "골드 부족하거나 오류";
                }
            }
            RefreshUI();
        }
    }
}