using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.TakeDamage(damage);
        }
        
        Debug.Log("플레이어 피격");
    }
}
