using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    private float damage;
    private readonly HashSet<IDamageable> damagedEnemies = new HashSet<IDamageable>();

    void OnEnable()
    {
        damagedEnemies.Clear();
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null && !damagedEnemies.Contains(damageable))
        {
            damageable.TakeDamage(damage);
            damagedEnemies.Add(damageable);
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }
}
