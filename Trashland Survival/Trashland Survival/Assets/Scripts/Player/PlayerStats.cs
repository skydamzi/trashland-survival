using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [Header("Calculated Stats (Read Only)")]
    [field: SerializeField] public float maxHP { get; private set; }
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public float attackPower { get; private set; }
    [field: SerializeField] public float coolDown { get; private set; }
    [field: SerializeField] public float magnetPower { get; private set; }
    [field: SerializeField] public float attackRange { get; private set; }

    [Header("Base Stats")]
    [SerializeField] private float baseMaxHP = 100f;
    [SerializeField] private float baseMoveSpeed = 2f;
    [SerializeField] private float baseAttackPower = 10f;
    [SerializeField] private float baseCoolDown = 1f;
    [SerializeField] private float baseMagnetPower = 1f;
    [SerializeField] private float baseAttackRange = 3f;

    [Header("Gold")]
    [SerializeField] private int goldAmount = 0;
    public int gold { get { return goldAmount; } private set { goldAmount = value; } }
    
    public void ResetStats()
    {
        gold = 0;
        CalculateStats(new List<UpgradeData>(), new List<EquipmentData>());
    }

    public void AddGold(int amount)
    {
        gold += amount;
        // 나중에 골드 변경은 이벤트로 처리하기
    }

    public void CalculateStats(List<UpgradeData> acquiredStatUpgrades, List<EquipmentData> acquiredEquipment)
    {
        float augmentHealthPercent = 0, augmentMoveSpeedPercent = 0, augmentAttackPowerPercent = 0,
              augmentCooldownPercent = 0, augmentMagnetPercent = 0, augmentAttackRangePercent = 0;

        foreach (var statUpgrade in acquiredStatUpgrades)
        {
            switch (statUpgrade.upgradeType)
            {
                case UpgradeType.Health: augmentHealthPercent += statUpgrade.value; break;
                case UpgradeType.AttackPower: augmentAttackPowerPercent += statUpgrade.value; break;
                case UpgradeType.MoveSpeed: augmentMoveSpeedPercent += statUpgrade.value; break;
                case UpgradeType.CooldownReduction: augmentCooldownPercent += statUpgrade.value; break;
                case UpgradeType.Magnet: augmentMagnetPercent += statUpgrade.value; break;
                case UpgradeType.AttackRange: augmentAttackRangePercent += statUpgrade.value; break;
            }
        }

        float augmentedMaxHP = baseMaxHP * (1 + augmentHealthPercent / 100f);
        float augmentedMoveSpeed = baseMoveSpeed * (1 + augmentMoveSpeedPercent / 100f);
        float augmentedAttackPower = baseAttackPower * (1 + augmentAttackPowerPercent / 100f);
        float augmentedCoolDown = baseCoolDown * (1 - augmentCooldownPercent / 100f);
        float augmentedMagnetPower = baseMagnetPower * (1 + augmentMagnetPercent / 100f);
        float augmentedAttackRange = baseAttackRange * (1 + augmentAttackRangePercent / 100f);

        float equipHealthPercent = 0, equipMoveSpeedPercent = 0, equipAttackPowerPercent = 0,
              equipCooldownPercent = 0, equipMagnetPercent = 0, equipAttackRangePercent = 0;

        foreach (var equip in acquiredEquipment)
        {
            equipHealthPercent += equip.healthBonus;
            equipMoveSpeedPercent += equip.moveSpeedBonus;
            equipAttackPowerPercent += equip.attackPowerBonus;
            equipCooldownPercent += equip.cooldownReduction;
            equipMagnetPercent += equip.magnetBonus;
            equipAttackRangePercent += equip.attackRangeBonus;
        }

        maxHP = augmentedMaxHP * (1 + equipHealthPercent / 100f);
        moveSpeed = augmentedMoveSpeed * (1 + equipMoveSpeedPercent / 100f);
        attackPower = augmentedAttackPower * (1 + equipAttackPowerPercent / 100f);
        coolDown = augmentedCoolDown * (1 - equipCooldownPercent / 100f);
        magnetPower = augmentedMagnetPower * (1 + equipMagnetPercent / 100f);
        attackRange = augmentedAttackRange * (1 + equipAttackRangePercent / 100f);
    }
}
