using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public Image iconImage;
    public Button button;

    private EquipmentData currentUpgrade;
    private UpgradeUI upgradeUI;

    public void Init(UpgradeUI parentUI)
    {
        upgradeUI = parentUI;
        button.onClick.AddListener(OnCardClicked);
    }

    public void SetData(EquipmentData data)
    {
        currentUpgrade = data;
        nameText.text = data.itemName;
        descriptionText.text = data.itemDescription;
        iconImage.sprite = data.itemIcon;
    }

    private void OnCardClicked()
    {
        if (currentUpgrade != null)
        {
            UpgradeManager.Instance.ApplyUpgrade(currentUpgrade);
            upgradeUI.Hide();
        }
    }
}
