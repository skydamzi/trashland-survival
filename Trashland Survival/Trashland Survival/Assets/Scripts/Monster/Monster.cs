using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public float health = 100f;
    public float attackPower = 10f;
    public GameObject EXPGemPrefab;
    public int expAmount = 1;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"몬스터가 {damage} 피해");
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        if (EXPGemPrefab != null)
        {
            GameObject gem = Instantiate(EXPGemPrefab, transform.position, Quaternion.identity);

            EXPGem expGem = gem.GetComponent<EXPGem>();
            if (expGem != null)
            {
                expGem.SetExperience(expAmount);
            } 
        }
        Destroy(gameObject);
    }
}
