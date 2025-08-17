using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private WeaponBase currentWeapon;
    private float nextFireTime = 0f;
    private float detectionRadius;
    private float fireRate;

    public Transform neckTransform;
    private bool isStretching = false;

    void Start()
    {
        string selectedType = PlayerManager.Instance.weaponType;
        if (selectedType == "Gun")
        {
            currentWeapon = GetComponentInChildren<Gun>();
        }
        /*
        else if (selectedType == "Punch")
        {
            currentWeapon = GetComponentInChildren<Punch>();
        }
        else if (selectedType == "Boomerang")
        {
            currentWeapon = GetComponentInChildren<Boomerang>();
        }
        */

        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(true);
        }

        detectionRadius = PlayerManager.Instance.attackRange;
        fireRate = PlayerManager.Instance.coolDown;
    }

    void Update()
    {
        Transform target = FindNearestMonster();
        LookAtNearestMonster(target);
        
        if (Time.time >= nextFireTime)
        {
            if (target != null)
            {
                currentWeapon.Attack(target);
                nextFireTime = Time.time + fireRate;
                StartCoroutine(QuickStretch());
            }
        }
    }

    private Transform FindNearestMonster()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Monster"));
        if (monsters.Length == 0) return null;

        Transform nearestMonster = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D monster in monsters)
        {
            float distanceToMonster = Vector2.Distance(transform.position, monster.transform.position);
            if (distanceToMonster < shortestDistance)
            {
                shortestDistance = distanceToMonster;
                nearestMonster = monster.transform;
            }
        }
        return nearestMonster;
    }

    private void LookAtNearestMonster(Transform target)
    {
        if (target != null && neckTransform != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            neckTransform.rotation = Quaternion.Slerp(neckTransform.rotation, targetRotation, 0.1f);
        }
        else
        {
            neckTransform.rotation = Quaternion.Slerp(neckTransform.rotation, Quaternion.identity, 0.1f);
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