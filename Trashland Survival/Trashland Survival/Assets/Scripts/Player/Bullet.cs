using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private bool hasHit = false;

    void OnEnable()
    {
        hasHit = false;
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            hasHit = true;
            damageable.TakeDamage(damage);
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }
}