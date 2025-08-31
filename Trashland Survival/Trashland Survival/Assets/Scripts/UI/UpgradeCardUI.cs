using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public Image iconImage;
    public Button button;
    public Outline outlineEffect;

    private EquipmentData currentEquipmentUpgrade;
    private UpgradeData currentStatUpgrade;
    public UpgradeUI parentUI;

    public void Init(UpgradeUI parentUI)
    {
        this.parentUI = parentUI;
        button.onClick.AddListener(OnCardClicked);
        if (outlineEffect != null)
        {
            outlineEffect.enabled = true;
        }
    }

    public void SetData(EquipmentData data)
    {
        currentEquipmentUpgrade = data;
        currentStatUpgrade = null;
        nameText.text = data.itemName;
        descriptionText.text = data.itemDescription;
        iconImage.sprite = data.itemIcon;
        UpdateOutlineEffect(data.rarity);
    }

    public void SetData(UpgradeData data)
    {
        currentStatUpgrade = data;
        currentEquipmentUpgrade = null;
        nameText.text = data.upgradeName;
        descriptionText.text = data.upgradeDescription;
        iconImage.sprite = data.upgradeIcon;

        if (outlineEffect != null)
        {
            outlineEffect.enabled = true;
            outlineEffect.effectColor = Color.green;
        }
    }

    private void UpdateOutlineEffect(ItemRarity rarity)
    {
        if (outlineEffect == null) return;

        outlineEffect.enabled = true;
        switch (rarity)
        {
            case ItemRarity.Normal:
                outlineEffect.effectColor = Color.gray;
                break;
            case ItemRarity.Rare:
                outlineEffect.effectColor = Color.blue;
                break;
            case ItemRarity.Unique:
                outlineEffect.effectColor = new Color(0.7f, 0f, 1f);
                break;
            case ItemRarity.Legendary:
                outlineEffect.effectColor = Color.yellow;
                break;
            default:
                outlineEffect.enabled = false;
                break;
        }
    }

    private void OnCardClicked()
    {
        if (currentEquipmentUpgrade != null)
        {
            UpgradeManager.Instance.ApplyUpgrade(currentEquipmentUpgrade);
            parentUI.OnCardSelected(this);
        }
        else if (currentStatUpgrade != null)
        {
            UpgradeManager.Instance.ApplyUpgrade(currentStatUpgrade);
            parentUI.OnCardSelected(this);
        }
    }
}