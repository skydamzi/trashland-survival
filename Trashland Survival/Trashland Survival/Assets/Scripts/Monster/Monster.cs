using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour, IDamageable
{
    public MonsterData monsterData;
    private float currentHealth;
    private Transform playerTransform;

    private EliteMonsterData eliteData;
    private bool isEliteWithCharge = false;
    private bool isAttacking = false;
    private float attackCooldown = 5f;
    private float lastAttackTime;
    public float chargeRange = 10f;
    public float indicatorWidth = 0.2f;
    public float chargeSpeedMultiplier = 8f;

    [Header("엘리트 몬스터 전용")]
    public GameObject chargeIndicatorPrefab;

    void OnEnable()
    {
        if (monsterData != null)
        {
            currentHealth = monsterData.health;
            eliteData = monsterData as EliteMonsterData;
            if (eliteData != null && eliteData.attackPatterns.Contains(EliteAttackPattern.Charge))
            {
                isEliteWithCharge = true;
                lastAttackTime = Time.time;
            }
            else
            {
                isEliteWithCharge = false;
            }
        }

        if (PlayerManager.Instance != null)
        {
            playerTransform = PlayerManager.Instance.playerTransform;
        }
        isAttacking = false;
    }

    void Update()
    {
        if (playerTransform == null || monsterData == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (isEliteWithCharge && !isAttacking && Time.time >= lastAttackTime + attackCooldown && distanceToPlayer <= chargeRange)
        {
            StartCoroutine(ChargeAttack());
        }

        if (!isAttacking)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * monsterData.moveSpeed * Time.deltaTime;
        }
    }

    private IEnumerator ChargeAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        Vector3 targetPosition = playerTransform.position;
        Vector3 startPosition = transform.position;
        Vector3 direction = (targetPosition - startPosition).normalized;

        if (chargeIndicatorPrefab != null)
        {
            float distance = Vector3.Distance(startPosition, targetPosition);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject indicator = Instantiate(chargeIndicatorPrefab, startPosition, rotation);

            indicator.transform.localScale = new Vector3(distance, indicatorWidth, 1f);
            indicator.transform.position = startPosition + direction * distance / 2;
            
            yield return new WaitForSeconds(1f);
            Destroy(indicator);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        float chargeSpeed = monsterData.moveSpeed * chargeSpeedMultiplier;
        float chargeDuration = 0.5f;
        float timer = 0f;

        while (timer < chargeDuration)
        {
            transform.position += direction * chargeSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
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
}
