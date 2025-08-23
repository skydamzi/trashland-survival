using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public MonsterData monsterData;
    private float currentHealth;
    private Transform playerTransform;

    void OnEnable()
    {
        if (monsterData != null) currentHealth = monsterData.health;
        if (PlayerManager.Instance != null) playerTransform = PlayerManager.Instance.playerTransform;
    }

    void Update()
    {
        if (playerTransform != null && monsterData != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * monsterData.moveSpeed * Time.deltaTime;
        }
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
