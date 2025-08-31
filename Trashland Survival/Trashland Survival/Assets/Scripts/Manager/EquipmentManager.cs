using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class SerializableEquippedItem
{
    public EquipmentType type;
    public string itemName;
}

[System.Serializable]
public class SerializableEquippedItems
{
    public List<SerializableEquippedItem> items = new List<SerializableEquippedItem>();
}

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public List<EquipmentData> allEquipment;
    public Dictionary<EquipmentType, List<EquipmentData>> equippedItems = new Dictionary<EquipmentType, List<EquipmentData>>();
    
    public static event Action OnEquipmentChanged;

    private const int MaxAccessories = 4;
    private const string EquippedItemsSaveKey = "EquippedItems";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEmptyLists();
            //LoadAllEquipmentData();
            //LoadEquippedItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEmptyLists()
    {
        equippedItems[EquipmentType.Accessory] = new List<EquipmentData>();
        equippedItems[EquipmentType.Shoes] = new List<EquipmentData>();
    }


    public void EquipItem(EquipmentData itemToEquip)
    {
        if (itemToEquip == null) return;

        EquipmentType type = itemToEquip.equipmentType;

        if (type == EquipmentType.Accessory)
        {
            if (equippedItems[type].Count < MaxAccessories)
            {
                equippedItems[type].Add(itemToEquip);
                Debug.Log($"악세사리 장착: {itemToEquip.itemName}");
                SaveEquippedItems();
                OnEquipmentChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning("장착 불가능: " + MaxAccessories + "개 초과");
            }
        }
        else if (type == EquipmentType.Shoes)
        {
            equippedItems[type].Clear();
            equippedItems[type].Add(itemToEquip);
            Debug.Log($"신발 장착: {itemToEquip.itemName}");
            SaveEquippedItems();
            OnEquipmentChanged?.Invoke();
        }
    }

    public void UnequipItem(EquipmentData itemToUnequip)
    {
        if (itemToUnequip == null) return;

        EquipmentType type = itemToUnequip.equipmentType;

        if (equippedItems.ContainsKey(type) && equippedItems[type].Contains(itemToUnequip))
        {
            equippedItems[type].Remove(itemToUnequip);
            Debug.Log($"탈착: {itemToUnequip.itemName}");
            SaveEquippedItems();
            OnEquipmentChanged?.Invoke();
        }
    }

    public List<EquipmentData> GetEquippedItems(EquipmentType slotType)
    {
        if (equippedItems.TryGetValue(slotType, out List<EquipmentData> items))
        {
            return items;
        }
        return new List<EquipmentData>();
    }

    public void SaveEquippedItems()
    {
        SerializableEquippedItems wrapper = new SerializableEquippedItems();
        foreach (var pair in equippedItems)
        {
            foreach (var itemData in pair.Value)
            {
                if (itemData != null)
                {
                    wrapper.items.Add(new SerializableEquippedItem { type = pair.Key, itemName = itemData.itemName });
                }
            }
        }

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(EquippedItemsSaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadEquippedItems()
    {
        if (PlayerPrefs.HasKey(EquippedItemsSaveKey))
        {
            string json = PlayerPrefs.GetString(EquippedItemsSaveKey);
            if (string.IsNullOrEmpty(json)) return;

            SerializableEquippedItems wrapper = JsonUtility.FromJson<SerializableEquippedItems>(json);
            
            InitializeEmptyLists();

            foreach (var savedItem in wrapper.items)
            {
                EquipmentData itemData = FindEquipmentDataByName(savedItem.itemName);
                if (itemData != null)
                {
                    EquipItem(itemData);
                }
                else
                {
                    Debug.LogWarning($"찾을 수 없는 아이템: {savedItem.itemName}");
                }
            }
            SaveEquippedItems();
        }
    }

    private EquipmentData FindEquipmentDataByName(string name)
    {
        return allEquipment.Find(item => item.itemName == name);
    }
}