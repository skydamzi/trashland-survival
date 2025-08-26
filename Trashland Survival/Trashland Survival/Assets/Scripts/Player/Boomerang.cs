using UnityEngine;
using System.Collections;

public class Boomerang : WeaponBase
{
    public GameObject boomerangPrefab;
    private bool isBoomerangActive = false;
    private Renderer neckRenderer;
    private Transform neckTransform;

    void Start()
    {
        PlayerAttackController playerAttackController = GetComponentInParent<PlayerAttackController>();
        if (playerAttackController != null && playerAttackController.neckTransform != null)
        {
            neckTransform = playerAttackController.neckTransform;
            neckRenderer = neckTransform.GetComponent<Renderer>();
        }
    }

    public override void Attack(Transform target)
    {
        if (isBoomerangActive || boomerangPrefab == null) return;

        isBoomerangActive = true;
        
        if (neckRenderer != null)
        {
            neckRenderer.enabled = false;
        }

        // Instantiate 대신 GetFromPool 사용
        GameObject boomerangInstance = PoolManager.Instance.GetFromPool(boomerangPrefab, transform.position, Quaternion.identity);
        if (boomerangInstance == null) 
        {
            isBoomerangActive = false; // 풀에서 가져오기 실패 시 공격 상태 초기화
            return;
        }

        BoomerangProjectile projectile = boomerangInstance.GetComponent<BoomerangProjectile>();

        if (projectile != null)
        {
            projectile.Initialize(this, target, PlayerManager.Instance.attackPower, PlayerManager.Instance.attackRange);
        }
    }

    public void OnBoomerangReturned(Quaternion finalProjectileRotation)
    {
        isBoomerangActive = false;
        if (neckTransform != null)
        {
            StartCoroutine(SettleRotation(finalProjectileRotation));
        }
    }

    private IEnumerator SettleRotation(Quaternion startRotation)
    {
        if (neckRenderer != null)
        {
            neckRenderer.enabled = true;
        }

        float duration = 0.2f; 
        float timer = 0f;

        Quaternion endRotation = neckTransform.parent.rotation;

        while (timer < duration)
        {
            neckTransform.rotation = Quaternion.Slerp(startRotation, endRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}

