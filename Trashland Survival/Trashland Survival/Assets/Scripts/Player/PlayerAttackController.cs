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
    public bool IsAttacking { get; set; }

    void Start()
    {
        string selectedType = PlayerManager.Instance.weaponType;
        if (selectedType == "Gun")
        {
            currentWeapon = GetComponentInChildren<Gun>();
        }
        else if (selectedType == "Punch")
        {
            currentWeapon = GetComponentInChildren<Punch>();
        }
        else if (selectedType == "Boomerang")
        {
            currentWeapon = GetComponentInChildren<Boomerang>();
        }

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

        if (!IsAttacking)
        {
            LookAtNearestMonster(target);
        }
        
        if (Time.time >= nextFireTime)
        {
            if (target != null)
            {
                currentWeapon.Attack(target);
                nextFireTime = Time.time + fireRate;
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
}