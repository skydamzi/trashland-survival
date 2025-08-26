using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : WeaponBase
{
    public Transform neckTransform;
    private PlayerAttackController attackController;

    void Awake()
    {
        attackController = GetComponentInParent<PlayerAttackController>();
        if (neckTransform == null)
        {
            neckTransform = attackController.neckTransform;
        }
    }

    public override void Attack(Transform target)
    {
        if (attackController.IsAttacking) return;
        StartCoroutine(PunchCoroutine(target));
    }

    private IEnumerator PunchCoroutine(Transform target)
    {
        attackController.IsAttacking = true;

        float aimDuration = 0.1f;
        float timer = 0f;
        Quaternion startRotation = neckTransform.rotation;
        
        Vector2 direction = (target.position - neckTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        while (timer < aimDuration)
        {
            neckTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, timer / aimDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        neckTransform.rotation = targetRotation;

        float distanceToTarget = Vector2.Distance(neckTransform.position, target.position);
        float punchLength = Mathf.Min(distanceToTarget, PlayerManager.Instance.attackRange);

        Vector3 originalScale = Vector3.one;
        neckTransform.localScale = originalScale;

        Vector3 slightStretchScale = new Vector3(originalScale.x, punchLength * 0.3f, originalScale.z);
        Vector3 maxStretchScale = new Vector3(originalScale.x, punchLength, originalScale.z);

        float slightStretchDuration = 0.2f;
        float maxStretchDuration = 0.1f;
        float holdDuration = 0.05f;
        float returnDuration = 0.4f;

        timer = 0f;
        while (timer < slightStretchDuration)
        {
            neckTransform.localScale = Vector3.Lerp(originalScale, slightStretchScale, timer / slightStretchDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < maxStretchDuration)
        {
            timer += Time.deltaTime;
            neckTransform.localScale = Vector3.Lerp(slightStretchScale, maxStretchScale, timer / maxStretchDuration);
            yield return null;
        }
        neckTransform.localScale = maxStretchScale;

        yield return new WaitForSeconds(holdDuration);

        Vector2 boxCenter = (Vector2)neckTransform.position + (direction * (punchLength / 2));
        Vector2 boxSize = new Vector2(neckTransform.lossyScale.x, punchLength);
        int layerMask = LayerMask.GetMask("Monster");

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle - 90f, layerMask);
        HashSet<IDamageable> hitEnemies = new HashSet<IDamageable>();

        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponentInParent<IDamageable>();
            if (damageable != null && !hitEnemies.Contains(damageable))
            {
                if ((MonoBehaviour)damageable != (MonoBehaviour)this && ((MonoBehaviour)damageable).transform.root != transform.root)
                {
                    damageable.TakeDamage(PlayerManager.Instance.attackPower);
                    hitEnemies.Add(damageable);
                }
            }
        }

        timer = 0f;
        while (timer < returnDuration)
        {
            neckTransform.localScale = Vector3.Lerp(maxStretchScale, originalScale, timer / returnDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        neckTransform.localScale = originalScale;

        attackController.IsAttacking = false;
    }
}