using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }
}