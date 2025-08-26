
using UnityEngine;

public enum ItemRarity
{
    Normal,
    Rare,
    Unique,
    Legendary
}

public enum EquipmentType
{
    RangedWeapon,
    MeleeWeapon,
    AreaWeapon,
    Accessory,
    Shoes
}

[CreateAssetMenu(fileName = "New EquipmentData", menuName = "Data/Equipment Data")]
public class EquipmentData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemIcon;
    public ItemRarity rarity;
    public EquipmentType equipmentType;

    [Header("능력치 보너스")]
    public float healthBonus;
    public float expBonus;
    public float moveSpeedBonus;
    public float attackPowerBonus;
    public float cooldownReduction;
    public float magnetBonus;
    public float attackRangeBonus;
}
