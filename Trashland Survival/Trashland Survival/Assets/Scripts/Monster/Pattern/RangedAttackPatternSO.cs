using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "RangedAttackPattern", menuName = "Data/Attack Patterns/Ranged")]
public class RangedAttackPatternSO : AttackPatternSO
{
    [Header("원거리 공격 속성")]
    public GameObject projectilePrefab;
    public float attackRange = 8f;
    public float projectileSpeed = 10f;
    public float attackAnimDelay = 0.5f; // 공격 후 딜레이

    public override void Execute(Monster monster)
    {
        monster.StartCoroutine(Attack(monster));
    }

    private IEnumerator Attack(Monster monster)
    {
        monster.isAttacking = true;

        if (monster.playerTransform != null)
        {
            Vector3 direction = (monster.playerTransform.position - monster.transform.position).normalized;
            GameObject projectileGO = PoolManager.Instance.GetFromPool(projectilePrefab, monster.transform.position, Quaternion.identity);
            if (projectileGO != null)
            {
                Projectile projectile = projectileGO.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.Initialize(direction, projectileSpeed, monster.monsterData.attackPower, monster.gameObject);
                }
            }
        }

        yield return new WaitForSeconds(attackAnimDelay);
        monster.isAttacking = false;
    }
}