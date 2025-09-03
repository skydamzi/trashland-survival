using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour, IDamageable
{
    public MonsterData monsterData { get; private set; }
    private float currentHealth;
    public Transform playerTransform { get; private set; }

    private EliteMonsterData eliteData;
    public bool isAttacking { get; set; }

    private Dictionary<AttackPatternSO, float> attackCooldowns = new Dictionary<AttackPatternSO, float>();
    private List<GameObject> spawnedPatternObjects = new List<GameObject>();

    public void Initialize(MonsterData data, Transform target)
    {
        monsterData = data;
        currentHealth = monsterData.health;
        eliteData = monsterData as EliteMonsterData;

        playerTransform = target;
        spawnedPatternObjects.Clear();

        if (eliteData != null)
        {
            foreach (var pattern in eliteData.attackPatterns)
            {
                attackCooldowns[pattern] = -pattern.cooldown;
            }
        }
        isAttacking = false;
    }

    void Update()
    {
        if (playerTransform == null || monsterData == null || isAttacking) return;

        if (eliteData != null)
        {
            foreach (var pattern in eliteData.attackPatterns)
            {
                if (Time.time >= attackCooldowns[pattern] + pattern.cooldown)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                    if (distanceToPlayer <= GetAttackRange(pattern))
                    {
                        attackCooldowns[pattern] = Time.time;
                        pattern.Execute(this);
                        return;
                    }
                }
            }
        }

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * monsterData.moveSpeed * Time.deltaTime;
    }

    private float GetAttackRange(AttackPatternSO pattern)
    {
        if (pattern is RangedAttackPatternSO ranged)
        {
            return ranged.attackRange;
        }
        if (pattern is ChargeAttackPatternSO charge)
        {
            return charge.chargeRange;
        }
        if (pattern is AreaAttackPatternSO area)
        {
            return area.attackRange;
        }
        return float.MaxValue;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        foreach (var obj in spawnedPatternObjects)
        {
            if (obj != null && PoolManager.Instance != null)
            {
                PoolManager.Instance.ReturnToPool(obj);
            }
        }
        spawnedPatternObjects.Clear();

        if (PoolManager.Instance != null && monsterData != null && monsterData.expGemPrefab != null && monsterData.expAmount > 0)
        {
            GameObject gem = PoolManager.Instance.GetFromPool(monsterData.expGemPrefab, transform.position, Quaternion.identity);
            if (gem != null)
            {
                EXPGem expGem = gem.GetComponent<EXPGem>();
                if (expGem != null)
                {
                    expGem.SetExperience(monsterData.expAmount);
                }
            }
        }
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    public float GetDamage()
    {
        return monsterData != null ? monsterData.attackPower : 0;
    }

    public GameObject GetOwner()
    {
        return gameObject;
    }
    
    public void RegisterSpawnedPatternObject(GameObject obj)
    {
        if (obj != null)
        {
            spawnedPatternObjects.Add(obj);
        }
    }
}