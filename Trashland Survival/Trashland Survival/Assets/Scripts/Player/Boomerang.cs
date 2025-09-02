using System;
using System.Collections;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public GameObject boomerangPrefab;
    public Transform weaponSpawnPoint;
    public float boomerangSpeed = 10f;
    public float boomerangLifeTime = 3f;
    public AudioClip fireSound;
    public int triggerCount = 3;
    private int attackCount = 0;
    private PlayerAttackController playerAttackController;
    private bool isBoomerangActive = false;

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

    public void EnableBoomerang()
    {
        this.enabled = true;
        attackCount = 0;
        Debug.Log("부메랑 활성화");
    }

    public void DisableBoomerang()
    {
        this.enabled = false;
        Debug.Log("부메랑 비활성화");
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
            Debug.Log("부메랑 카운트: " + attackCount + "/" + triggerCount);
        }
    }

    public void Attack(Transform target)
    {
        if (isBoomerangActive || boomerangPrefab == null) return;

        isBoomerangActive = true;

        GameObject boomerangInstance = PoolManager.Instance.GetFromPool(boomerangPrefab, weaponSpawnPoint.position, Quaternion.identity);
        if (boomerangInstance == null) 
        {
            isBoomerangActive = false;
            return;
        }

        if (SoundManager.Instance != null && fireSound != null)
        {
            SoundManager.Instance.PlaySFX(fireSound);
        }

        BoomerangBullet bullet = boomerangInstance.GetComponent<BoomerangBullet>();

        if (bullet != null)
        {
            bullet.Initialize(this, target, PlayerManager.Instance.attackPower, PlayerManager.Instance.attackRange, boomerangLifeTime);
        }
    }

    public void OnBoomerangReturned()
    {
        isBoomerangActive = false;
    }
}