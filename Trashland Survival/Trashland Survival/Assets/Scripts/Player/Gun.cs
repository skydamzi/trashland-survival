using UnityEngine;

public class Gun : WeaponBase
{
    public GameObject bulletPrefab;
    public Transform weaponSpawnPoint;
    public float bulletSpeed = 30f;
    public float bulletLifeTime = 1f;

    void Start()
    {
        damage = PlayerManager.Instance.attackPower;   
    }

    public override void Attack(Transform target)
    {
        Vector2 direction = (target.position - weaponSpawnPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, weaponSpawnPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDamage(damage);
        }

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction * bulletSpeed;

            Collider2D[] playerCols = GetComponentsInParent<Collider2D>();
            Collider2D bulletCol = bullet.GetComponent<Collider2D>();

            if (bulletCol != null)
            {
                foreach (var col in playerCols)
                {
                    if (col != null)
                    {
                        Physics2D.IgnoreCollision(bulletCol, col);
                    }
                }
            }
        }
        Destroy(bullet, bulletLifeTime);
    }
}