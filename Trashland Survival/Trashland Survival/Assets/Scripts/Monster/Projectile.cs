using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damage;
    private GameObject owner;
    private float lifetime = 5f;

    public void Initialize(Vector3 dir, float spd, float dmg, GameObject projOwner)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
        owner = projOwner;
        
        Invoke(nameof(ReturnToPool), lifetime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if(collision.CompareTag("Player"))
            {
                damageable.TakeDamage(damage);
            }
        }
        
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (gameObject.activeInHierarchy)
        {
            CancelInvoke();
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }
}