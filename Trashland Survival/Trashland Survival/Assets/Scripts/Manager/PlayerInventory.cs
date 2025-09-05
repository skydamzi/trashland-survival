using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SerializableInventoryItem
{
    public string itemName;
}

[System.Serializable]
public class SerializableInventory
{
    public List<SerializableInventoryItem> items = new List<SerializableInventoryItem>();
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public List<EquipmentData> ownedItems = new List<EquipmentData>();
    private const string InventorySaveKey = "PlayerInventory";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(EquipmentData item)
    {
        if (item != null && !ownedItems.Contains(item))
        {
            ownedItems.Add(item);
            Debug.Log($"{item.itemName} 추가");
            SaveInventory();
        }
    }

    public void RemoveItem(EquipmentData item)
    {
        if (item != null && ownedItems.Contains(item))
        {
            ownedItems.Remove(item);
            Debug.Log($"{item.itemName} 제거");
            SaveInventory();
        }
    }

    public void SaveInventory()
    {
        SerializableInventory wrapper = new SerializableInventory();
        foreach (var itemData in ownedItems)
        {
            if (itemData != null)
            {
                wrapper.items.Add(new SerializableInventoryItem { itemName = itemData.itemName });
            }
        }

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(InventorySaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey(InventorySaveKey))
        {
            string json = PlayerPrefs.GetString(InventorySaveKey);
            if (string.IsNullOrEmpty(json)) return;

            SerializableInventory wrapper = JsonUtility.FromJson<SerializableInventory>(json);
            ownedItems.Clear();

            foreach (var savedItem in wrapper.items)
            {
                EquipmentData itemData = FindEquipmentDataByName(savedItem.itemName);
                if (itemData != null)
                {
                    ownedItems.Add(itemData);
                }
                else
                {
                    Debug.LogWarning($"찾을 수 없는 아이템: {savedItem.itemName}");
                }
            }
        }
    }

    private EquipmentData FindEquipmentDataByName(string name)
    {
        if (EquipmentManager.Instance != null && EquipmentManager.Instance.allEquipment != null)
        {
            return EquipmentManager.Instance.allEquipment.Find(item => item.itemName == name);
        }
        Debug.LogError("EquipmentManager 또는 마스터 목록을 이용할 수 없음");
        return null;
    }
}