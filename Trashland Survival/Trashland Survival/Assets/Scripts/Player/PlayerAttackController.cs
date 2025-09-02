using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Gun gun;
    private float nextFireTime = 0f;

    public Transform neckTransform;
    public bool IsAttacking { get; set; }

    public float neckRotationSpeed = 10f;

    void Start()
    {
        gun = GetComponentInChildren<Gun>(true);
        gun.gameObject.SetActive(true);
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
                gun.Attack(target);
                nextFireTime = Time.time + PlayerManager.Instance.coolDown;
            }
        }
    }

    public Transform FindNearestMonster()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, PlayerManager.Instance.attackRange, LayerMask.GetMask("Monster"));
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
            
            neckTransform.rotation = Quaternion.Slerp(neckTransform.rotation, targetRotation, neckRotationSpeed * Time.deltaTime);
        }
        else
        {
            neckTransform.rotation = Quaternion.Slerp(neckTransform.rotation, Quaternion.identity, neckRotationSpeed * Time.deltaTime);
        }
    }
}