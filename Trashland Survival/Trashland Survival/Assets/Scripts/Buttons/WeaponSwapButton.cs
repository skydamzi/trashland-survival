using UnityEngine;
using UnityEngine.UI;

public class WeaponSwapButton : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SwapWeapon);
    }

    public void SwapWeapon()
    {
        GameEvents.OnWeaponSwapRequested.Invoke();
    }
}
