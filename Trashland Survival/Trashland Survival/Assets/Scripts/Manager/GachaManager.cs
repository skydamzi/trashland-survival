using UnityEngine;
using System.Collections.Generic;

public class GachaManager : MonoBehaviour
{
    public static GachaManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public EquipmentData PerformGacha(GachaPoolData pool)
    {
        if (PlayerManager.Instance == null || pool == null || PlayerInventory.Instance == null) return null;

        if (PlayerManager.Instance.gold < pool.costGold)
        {
            Debug.Log("골드 부족");
            return null;
        }

        if (!PlayerManager.Instance.SpendGold(pool.costGold))
        {
             return null;
        }

        float totalWeight = 0f;
        foreach (var entry in pool.items)
        {
            totalWeight += entry.weight;
        }

        float randomPoint = Random.value * totalWeight;
        float currentWeight = 0f;
        EquipmentData selectedItem = null;

        foreach (var entry in pool.items)
        {
            currentWeight += entry.weight;
            if (randomPoint <= currentWeight)
            {
                selectedItem = entry.equipment;
                break;
            }
        }

        if (selectedItem == null)
        {
            Debug.LogWarning("뽑기 오류 발생");
            PlayerManager.Instance.GainGold(pool.costGold); 
            return null;
        }

        PlayerInventory.Instance.AddItem(selectedItem);
        Debug.Log($"뽑은 템: {selectedItem.itemName} 인벤토리 추가");

        return selectedItem;
    }
}