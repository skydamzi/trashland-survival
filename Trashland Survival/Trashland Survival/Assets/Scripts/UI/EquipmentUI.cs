using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class EquipmentUI : MonoBehaviour
{
    public Transform inventoryContent;
    public GameObject inventoryItemPrefab;

    public List<Image> accessorySlots = new List<Image>(4);
    public Image shoesSlot;

    void OnEnable()
    {
        RefreshUI();
    }
    
    public void RefreshUI()
    {
        PopulateInventory();
        UpdateEquippedSlots();
    }

    void PopulateInventory()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        if (PlayerInventory.Instance == null || EquipmentManager.Instance == null) return;

        var allEquipped = EquipmentManager.Instance.equippedItems.Values.SelectMany(list => list).ToList();

        foreach (var itemData in PlayerInventory.Instance.ownedItems)
        {
            if (!allEquipped.Contains(itemData))
            {
                if (itemData.equipmentType == EquipmentType.Accessory || itemData.equipmentType == EquipmentType.Shoes)
                {
                    GameObject itemGO = Instantiate(inventoryItemPrefab, inventoryContent);
                    itemGO.GetComponent<Image>().sprite = itemData.itemIcon;
                    Button itemButton = itemGO.GetComponent<Button>();
                    if (itemButton != null)
                    {
                        itemButton.onClick.AddListener(() => OnInventoryItemClick(itemData));
                    }
                }
            }
        }
    }

    void UpdateEquippedSlots()
    {
        UpdateAccessorySlots();
        UpdateShoeSlot();
    }

    void UpdateAccessorySlots()
    {
        List<EquipmentData> equippedAccessories = EquipmentManager.Instance.GetEquippedItems(EquipmentType.Accessory);

        for (int i = 0; i < accessorySlots.Count; i++)
        {
            Image slotImage = accessorySlots[i];
            Button slotButton = slotImage.GetComponent<Button>();
            slotButton.onClick.RemoveAllListeners();

            if (i < equippedAccessories.Count)
            {
                EquipmentData equippedItem = equippedAccessories[i];
                slotImage.sprite = equippedItem.itemIcon;
                slotImage.color = Color.white;
                slotButton.onClick.AddListener(() => OnEquippedItemClick(equippedItem));
            }
            else
            {
                slotImage.sprite = null;
                slotImage.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

    void UpdateShoeSlot()
    {
        List<EquipmentData> equippedShoes = EquipmentManager.Instance.GetEquippedItems(EquipmentType.Shoes);
        Button slotButton = shoesSlot.GetComponent<Button>();
        slotButton.onClick.RemoveAllListeners();

        if (equippedShoes.Any())
        {
            EquipmentData equippedItem = equippedShoes.First();
            shoesSlot.sprite = equippedItem.itemIcon;
            shoesSlot.color = Color.white;
            slotButton.onClick.AddListener(() => OnEquippedItemClick(equippedItem));
        }
        else
        {
            shoesSlot.sprite = null;
            shoesSlot.color = new Color(1, 1, 1, 0.5f);
        }
    }

    void OnInventoryItemClick(EquipmentData itemData)
    {
        EquipmentManager.Instance.EquipItem(itemData);
        RefreshUI();
    }

    void OnEquippedItemClick(EquipmentData itemData)
    {
        EquipmentManager.Instance.UnequipItem(itemData);
        RefreshUI();
    }
}
