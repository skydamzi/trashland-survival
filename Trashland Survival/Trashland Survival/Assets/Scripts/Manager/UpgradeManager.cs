using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<EquipmentData> allEquipmentUpgrades;
    public List<UpgradeData> allStatUpgrades;
    private List<EquipmentData> availableEquipmentUpgrades;
    private List<UpgradeData> availableStatUpgrades;

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
        availableEquipmentUpgrades = new List<EquipmentData>(allEquipmentUpgrades);
        availableStatUpgrades = new List<UpgradeData>(allStatUpgrades);
    }

    public List<object> GetRandomUpgrades(int count)
    {
        List<object> randomUpgrades = new List<object>();
        List<object> allAvailableUpgrades = new List<object>();
        foreach (var equip in availableEquipmentUpgrades)
        {
            allAvailableUpgrades.Add(equip);
        }
        foreach (var stat in availableStatUpgrades)
        {
            allAvailableUpgrades.Add(stat);
        }

        if (allAvailableUpgrades.Count == 0) return randomUpgrades;

        List<object> tempList = new List<object>(allAvailableUpgrades);

        int numToPick = Mathf.Min(count, tempList.Count);
        for (int i = 0; i < numToPick; i++)
        {
            int randIndex = Random.Range(0, tempList.Count);
            randomUpgrades.Add(tempList[randIndex]);
            tempList.RemoveAt(randIndex);
        }

        return randomUpgrades;
    }

    public void ApplyUpgrade(object data)
    {
        if (data is EquipmentData equipmentData)
        {
            PlayerManager.Instance.acquiredUpgrades.Add(equipmentData);
            availableEquipmentUpgrades.Remove(equipmentData);
        }
        else if (data is UpgradeData upgradeData)
        {
            PlayerManager.Instance.acquiredStatUpgrades.Add(upgradeData);
        }
        PlayerManager.Instance.UpdateStats();
    }
}
