
using UnityEngine;

public enum UpgradeType
{
    Health,
    AttackPower,
    MoveSpeed,
    CooldownReduction,
    Magnet,
    AttackRange
}

[CreateAssetMenu(fileName = "New UpgradeData", menuName = "Data/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Info")]
    public string upgradeName;
    [TextArea]
    public string upgradeDescription;
    public Sprite upgradeIcon;

    [Header("Upgrade")]
    public UpgradeType upgradeType;
    public float value;
}
