using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<EquipmentData> allUpgrades;
    private List<EquipmentData> availableUpgrades;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        ResetAvailableUpgrades();
    }

    public void ResetAvailableUpgrades()
    {
        availableUpgrades = new List<EquipmentData>(allUpgrades);
    }

    public List<EquipmentData> GetRandomUpgrades(int count)
    {
        List<EquipmentData> randomUpgrades = new List<EquipmentData>();
        if (availableUpgrades.Count == 0) return randomUpgrades;

        List<EquipmentData> tempList = new List<EquipmentData>(availableUpgrades);

        int numToPick = Mathf.Min(count, tempList.Count);
        for (int i = 0; i < numToPick; i++)
        {
            int randIndex = Random.Range(0, tempList.Count);
            randomUpgrades.Add(tempList[randIndex]);
            tempList.RemoveAt(randIndex);
        }

        return randomUpgrades;
    }

    public void ApplyUpgrade(EquipmentData upgrade)
    {
        PlayerManager player = PlayerManager.Instance;
        if (player == null || upgrade == null) return;

        player.maxHP += upgrade.healthBonus;
        player.currentHP += upgrade.healthBonus;
        player.moveSpeed += upgrade.moveSpeedBonus;
        player.attackPower += upgrade.attackPowerBonus;
        player.coolDown -= upgrade.cooldownReduction;
        player.magnetPower += upgrade.magnetBonus;
        player.attackRange += upgrade.attackRangeBonus;

        player.acquiredUpgrades.Add(upgrade);
        
        if (availableUpgrades.Contains(upgrade))
        {
            availableUpgrades.Remove(upgrade);
        }
        
        Debug.Log($"{upgrade.itemName} 적용됨");
    }
}
