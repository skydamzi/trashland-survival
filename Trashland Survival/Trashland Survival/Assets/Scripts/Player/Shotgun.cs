using System;
using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform weaponSpawnPoint;
    public float bulletSpeed = 30f;
    public float bulletLifeTime = 1f;
    public AudioClip fireSound;
    public int triggerCount = 3; // 발동 주기
    public int numberOfShots = 3;
    public float spreadAngle = 30f;
    private int attackCount = 0;
    private PlayerAttackController playerAttackController;

    void Start()
    {
        playerAttackController = GetComponentInParent<PlayerAttackController>();
        this.enabled = false;
    }

    void OnEnable()
    {
        Gun.OnPlayerAttack += OnGunAttack;
        attackCount = 0;
    }

    void OnDisable()
    {
        Gun.OnPlayerAttack -= OnGunAttack;
    }

    public void EnableShotgun()
    {
        this.enabled = true;
        attackCount = 0;
        Debug.Log("샷건 활성화");
    }
    public void DisableShotgun()
    {
        this.enabled = false;
        Debug.Log("샷건 비활성화");
    }

    private void OnGunAttack()
    {
        attackCount++;
        if (attackCount >= triggerCount)
        {
            Debug.Log("attackCount: " + attackCount);
            Transform target = playerAttackController?.FindNearestMonster();
            Attack(target);
            attackCount = 0;
        }
        else
        {
            Debug.Log("샷건 카운트: " + attackCount + "/" + triggerCount);
        }
    }

    public void Attack(Transform target)
    {       
        Vector2 direction = (target.position - weaponSpawnPoint.position).normalized;
        
        for (int i = 0; i < numberOfShots; i++)
        {
            float angle = (i - (numberOfShots / 2f)) * (spreadAngle / (numberOfShots - 1));
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 spreadDirection = rotation * direction;

            GameObject bullet = PoolManager.Instance.GetFromPool(bulletPrefab, weaponSpawnPoint.position, Quaternion.identity);

            if (SoundManager.Instance != null && fireSound != null)
            {
                SoundManager.Instance.PlaySFX(fireSound);
            }

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(PlayerManager.Instance.attackPower * 0.5f);
            }

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = spreadDirection * bulletSpeed;

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
            StartCoroutine(ReturnBulletAfterTime(bullet, bulletLifeTime));
        }
    }

    private IEnumerator ReturnBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet.activeSelf)
        {
            PoolManager.Instance.ReturnToPool(bullet);
        }
    }
}