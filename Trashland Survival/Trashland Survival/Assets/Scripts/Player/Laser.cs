using System;
using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laserBulletPrefab;
    public Transform weaponSpawnPoint;
    public float laserDuration = 3f;
    public float dotDamageInterval = 0.1f;
    public float laserRange = 5f;
    public AudioClip fireSound;
    public int triggerCount = 5;
    private int attackCount = 0;
    private PlayerAttackController playerAttackController;
    private bool isLaserActive = false;

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

    public void EnableLaser()
    {
        this.enabled = true;
        attackCount = 0;
        Debug.Log("레이저 활성화");
    }

    public void DisableLaser()
    {
        this.enabled = false;
        Debug.Log("레이저 비활성화");
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
            Debug.Log("레이저 카운트: " + attackCount + "/" + triggerCount);
        }
    }

    public void Attack(Transform target)
    {
        if (isLaserActive || laserBulletPrefab == null) return;

        isLaserActive = true;

        if (SoundManager.Instance != null && fireSound != null)
        {
            SoundManager.Instance.PlaySFX(fireSound);
        }

        Vector2 direction = Vector2.zero;
        if (playerAttackController != null && playerAttackController.neckTransform != null)
        {
            direction = playerAttackController.neckTransform.up;
        }
        else
        {
            direction = weaponSpawnPoint.up;
        }

        GameObject laserInstance = PoolManager.Instance.GetFromPool(laserBulletPrefab, weaponSpawnPoint.position, Quaternion.identity);
        if (laserInstance != null)
        {
            LaserBullet laserBullet = laserInstance.GetComponent<LaserBullet>();
            if (laserBullet != null)
            {
                laserBullet.Initialize(
                    direction,
                    PlayerManager.Instance.attackPower * 0.05f,
                    laserRange,
                    laserDuration,
                    dotDamageInterval,
                    weaponSpawnPoint,
                    playerAttackController?.neckTransform
                );
            }
        }
        isLaserActive = false;
    }
}