using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AreaAttackPattern", menuName = "Data/Attack Patterns/Area")]
public class AreaAttackPatternSO : AttackPatternSO
{
    [Header("범위 공격 속성")]
    public float attackRange = 12f;
    public float attackRadius = 3f;
    public float warningDuration = 1f;
    public float attackDuration = 0.5f;
    public GameObject warningIndicatorPrefab;
    public GameObject damageAreaPrefab;

    public override void Execute(Monster monster)
    {
        monster.StartCoroutine(Attack(monster));
    }

    private IEnumerator Attack(Monster monster)
    {
        monster.isAttacking = true;

        Vector3 targetPosition = monster.playerTransform.position;

        GameObject warningIndicator = PoolManager.Instance.GetFromPool(warningIndicatorPrefab, targetPosition, Quaternion.identity);
        if (warningIndicator != null)
        {
            warningIndicator.transform.localScale = Vector3.one * attackRadius * 2;
        }

        yield return new WaitForSeconds(warningDuration);

        if (warningIndicator != null)
        {
            PoolManager.Instance.ReturnToPool(warningIndicator);
        }

        GameObject damageArea = PoolManager.Instance.GetFromPool(damageAreaPrefab, targetPosition, Quaternion.identity);
        if (damageArea != null)
        {
            damageArea.transform.localScale = Vector3.one * attackRadius * 2;
            AreaDamage areaDamage = damageArea.GetComponent<AreaDamage>();
            if (areaDamage != null)
            {
                areaDamage.Initialize(monster.GetDamage(), attackDuration, monster.gameObject);
            }
        }

        yield return new WaitForSeconds(attackDuration);

        monster.isAttacking = false;
    }
}
