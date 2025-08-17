using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSelectButton : MonoBehaviour
{
    public string weaponType;

    public void OnClickSelectWeapon()
    {
        PlayerManager.Instance.weaponType = weaponType;
    }
}