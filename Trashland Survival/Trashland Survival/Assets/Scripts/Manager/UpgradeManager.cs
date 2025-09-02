using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<EquipmentData> allEquipmentUpgrades;
    public List<UpgradeData> allStatUpgrades;
    public List<WeaponData> allWeaponUpgrades;
    private List<EquipmentData> availableEquipmentUpgrades;
    private List<UpgradeData> availableStatUpgrades;
    private List<WeaponData> availableWeaponUpgrades;

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
        availableWeaponUpgrades = new List<WeaponData>(allWeaponUpgrades);
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
        foreach (var weapon in availableWeaponUpgrades)
        {
            allAvailableUpgrades.Add(weapon);
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
        else if (data is WeaponData weaponData)
        {
            EnableWeapon(weaponData);
            availableWeaponUpgrades.Remove(weaponData);
        }
        PlayerManager.Instance.UpdateStats();
    }
    
    private void EnableWeapon(WeaponData weaponData)
    {
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            switch (weaponData.weaponType)
            {
                case WeaponType.Shotgun:
                    Shotgun shotgun = player.GetComponentInChildren<Shotgun>(true);
                    if (shotgun != null)
                    {
                        shotgun.EnableShotgun();
                    }
                    break;

                case WeaponType.Boomerang:
                    Boomerang boomerang = player.GetComponentInChildren<Boomerang>(true);
                    if (boomerang != null)
                    {
                        boomerang.EnableBoomerang();
                    }
                    break;
                case WeaponType.Laser:
                    Laser laser = player.GetComponentInChildren<Laser>(true);
                    if (laser != null)
                    {
                        laser.EnableLaser();
                    }
                    break;
                // 다른 무기 타입도 여기에 추가
            }
        }
    }
}
