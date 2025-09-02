using UnityEngine;

public class AreaDamage : MonoBehaviour, IDamageDealer
{
    private float damage;
    private GameObject owner;
    private float duration;
    private float creationTime;

    public void Initialize(float dmg, float dur, GameObject ownerGO)
    {
        damage = dmg;
        duration = dur;
        owner = ownerGO;
        creationTime = Time.time;
    }

    void Update()
    {
        if (Time.time > creationTime + duration)
        {
            if(gameObject.activeInHierarchy)
            {
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public GameObject GetOwner()
    {
        return owner;
    }

    private void OnTriggerEnter2D(Collider2D other)
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
