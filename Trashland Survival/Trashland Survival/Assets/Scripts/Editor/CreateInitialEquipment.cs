
using UnityEngine;
using UnityEditor;

public class CreateInitialEquipment
{
    [MenuItem("Tools/Create Initial Equipment")]
    public static void CreateEquipment()
    {
        string path = "Assets/Datas/EquipDatas/";

        // 1. Ranged Weapon
        EquipmentData rangedWeapon = ScriptableObject.CreateInstance<EquipmentData>();
        rangedWeapon.itemName = "실험용 1";
        rangedWeapon.rarity = ItemRarity.Normal;
        rangedWeapon.equipmentType = EquipmentType.RangedWeapon;
        AssetDatabase.CreateAsset(rangedWeapon, path + "실험용 1.asset");

        // 2. Melee Weapon
        EquipmentData meleeWeapon = ScriptableObject.CreateInstance<EquipmentData>();
        meleeWeapon.itemName = "실험용 2";
        meleeWeapon.rarity = ItemRarity.Normal;
        meleeWeapon.equipmentType = EquipmentType.MeleeWeapon;
        AssetDatabase.CreateAsset(meleeWeapon, path + "실험용 2.asset");

        // 3. Area Weapon
        EquipmentData areaWeapon = ScriptableObject.CreateInstance<EquipmentData>();
        areaWeapon.itemName = "실험용 3";
        areaWeapon.rarity = ItemRarity.Normal;
        areaWeapon.equipmentType = EquipmentType.AreaWeapon;
        AssetDatabase.CreateAsset(areaWeapon, path + "실험용 3.asset");

        // 4. Accessory
        EquipmentData accessory = ScriptableObject.CreateInstance<EquipmentData>();
        accessory.itemName = "실험용 4";
        accessory.rarity = ItemRarity.Normal;
        accessory.equipmentType = EquipmentType.Accessory;
        AssetDatabase.CreateAsset(accessory, path + "실험용 4.asset");

        // 5. Shoes
        EquipmentData shoes = ScriptableObject.CreateInstance<EquipmentData>();
        shoes.itemName = "실험용 5";
        shoes.rarity = ItemRarity.Normal;
        shoes.equipmentType = EquipmentType.Shoes;
        AssetDatabase.CreateAsset(shoes, path + "실험용 5.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        EditorUtility.DisplayDialog("장비 생성 완료", "초기 장비 5종이 생성되었습니다.", "확인");
    }
}
