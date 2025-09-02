using UnityEngine;

public enum WeaponType
{
    Shotgun,
    Boomerang,
    Laser,
    Grenade
}

[CreateAssetMenu(fileName = "New WeaponData", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    [TextArea]
    public string weaponDescription;
    public Sprite weaponIcon;

    [Header("Weapon")]
    public WeaponType weaponType;
}