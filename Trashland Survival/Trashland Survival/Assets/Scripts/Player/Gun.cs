using System;
using System.Collections;
using UnityEngine;

public class Gun : WeaponBase
{
    public static Action OnPlayerAttack;
    public GameObject bulletPrefab;
    public Transform weaponSpawnPoint;
    public float bulletSpeed = 30f;
    public float bulletLifeTime = 1f;
    public Transform neckTransform;
    public AudioClip fireSound;
    private bool isStretching = false;

    void Start()
    {
        PlayerAttackController playerAttackController = GetComponentInParent<PlayerAttackController>();
        if (playerAttackController != null)
        {
            neckTransform = playerAttackController.neckTransform;
        }
    }

    public override void Attack(Transform target)
    {
        StartCoroutine(QuickStretch());
        OnPlayerAttack?.Invoke();
        Vector2 direction = (target.position - weaponSpawnPoint.position).normalized;
        
        GameObject bullet = PoolManager.Instance.GetFromPool(bulletPrefab, weaponSpawnPoint.position, Quaternion.identity);
        if (bullet == null) return;

        if (SoundManager.Instance != null && fireSound != null)
        {
            SoundManager.Instance.PlaySFX(fireSound);
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDamage(PlayerManager.Instance.attackPower);
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
        StartCoroutine(ReturnBulletAfterTime(bullet, bulletLifeTime));
    }

    private IEnumerator ReturnBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet.activeSelf)
        {
            PoolManager.Instance.ReturnToPool(bullet);
        }
    }

    private IEnumerator QuickStretch()
    {
        if (isStretching) yield break;
        isStretching = true;
        Vector3 originalScale = Vector3.one;
        Vector3 stretchScale = new Vector3(1f, 1.5f, 1f);
        float duration = 0.05f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            neckTransform.localScale = Vector3.Lerp(originalScale, stretchScale, t / duration);
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            neckTransform.localScale = Vector3.Lerp(stretchScale, originalScale, t / duration);
            yield return null;
        }
        neckTransform.localScale = originalScale;
        isStretching = false;
    }
}