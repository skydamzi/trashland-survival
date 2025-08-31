using UnityEngine;

public class MonsterHitbox : MonoBehaviour, IDamageDealer
{
    private Monster monster;

    void Awake()
    {
        monster = GetComponentInParent<Monster>();
    }

    public float GetDamage()
    {
        return monster.monsterData != null ? monster.monsterData.attackPower : 0;
    }

    public GameObject GetOwner()
    {
        return monster.gameObject;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == GetOwner())
        {
            return;
        }

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.GetComponent<PlayerDamageable>() != null)
        {
            damageable.TakeDamage(GetDamage());
        }
    }
}