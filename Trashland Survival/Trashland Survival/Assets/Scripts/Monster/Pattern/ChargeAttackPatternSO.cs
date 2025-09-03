using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ChargeAttackPattern", menuName = "Data/Attack Patterns/Charge")]
public class ChargeAttackPatternSO : AttackPatternSO
{
    [Header("돌진 공격 속성")]
    public float chargeRange = 10f;
    public float indicatorWidth = 0.2f;
    public float chargeSpeedMultiplier = 8f;
    public float warningDuration = 0.75f;
    public GameObject warningSignPrefab;

    public override void Execute(Monster monster)
    {
        monster.StartCoroutine(Charge(monster));
    }

    private IEnumerator Charge(Monster monster)
    {
        monster.isAttacking = true;

        Vector3 direction = Vector3.zero;
        if (monster.playerTransform != null)
        {
            direction = (monster.playerTransform.position - monster.transform.position).normalized;
        }

        GameObject warningSign = null;
        if (warningSignPrefab != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
            warningSign = PoolManager.Instance.GetFromPool(warningSignPrefab, monster.transform.position, rotation);
            if (warningSign != null)
            {
                warningSign.transform.localScale = new Vector3(indicatorWidth, chargeRange, 1f);
                warningSign.transform.position += warningSign.transform.up * (chargeRange / 2);
                monster.RegisterSpawnedPatternObject(warningSign);
            }
        }

        yield return new WaitForSeconds(warningDuration);

        if (warningSign != null)
        {
            PoolManager.Instance.ReturnToPool(warningSign);
        }

        float chargeSpeed = monster.monsterData.moveSpeed * chargeSpeedMultiplier;
        float chargeDuration = chargeRange / chargeSpeed;
        float timer = 0f;

        while (timer < chargeDuration)
        {
            monster.transform.position += direction * chargeSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        monster.isAttacking = false;
    }
}