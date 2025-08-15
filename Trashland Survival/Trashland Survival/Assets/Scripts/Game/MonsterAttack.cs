using UnityEngine;

public class MonsterAttack : MonoBehaviour, IDamageDealer
{
    public float attackDamage = 10f;

    public float GetDamage()
    {
        return attackDamage;
    }
    public GameObject GetOwner()
    {
        return this.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damagableTarget = other.GetComponent<IDamageable>();

        if (damagableTarget != null)
        {
            damagableTarget.TakeDamage(GetDamage());
            Debug.Log($"{attackDamage}의 피해");
        }
    }
}
