using UnityEngine;

public class PlayerDamagable : MonoBehaviour, IDamageable
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
