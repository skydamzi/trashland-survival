using UnityEngine;
using UnityEngine.UI;

public class WeaponSwapButton : MonoBehaviour
{
    private Button button;
    private PlayerAttackController playerAttackController;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SwapWeapon);
        playerAttackController = FindFirstObjectByType<PlayerAttackController>();
    }

    public void SwapWeapon()
    {
        if (PlayerManager.Instance == null) return;

        string currentWeapon = PlayerManager.Instance.weaponType;
        string nextWeapon = "";

        switch (currentWeapon)
        {
            case "Gun":
                nextWeapon = "Punch";
                break;
            case "Punch":
                nextWeapon = "Boomerang";
                break;
            case "Boomerang":
                nextWeapon = "Gun";
                break;
            default:
                nextWeapon = "Gun";
                break;
        }

        PlayerManager.Instance.weaponType = nextWeapon;

        if (playerAttackController != null)
        {
            playerAttackController.UpdateWeapon();
        }
    }
}
