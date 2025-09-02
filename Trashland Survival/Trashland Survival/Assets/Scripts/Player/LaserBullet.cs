using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserBullet : MonoBehaviour
{
    private float damage;
    private float duration;
    private float dotInterval;
    private Vector2 direction;
    private Vector2 startPosition;
    private float range;
    private LineRenderer lineRenderer;
    private HashSet<IDamageable> hitEnemies = new HashSet<IDamageable>();
    private bool isFiring = false;
    private Transform followTarget;
    private Transform rotationTarget;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Vector2 dir, float dmg, float rng, float dur, float interval, Transform target, Transform rotTarget = null)
    {
        this.direction = dir;
        this.damage = dmg;
        this.range = rng;
        this.duration = dur;
        this.dotInterval = interval;
        this.followTarget = target;
        this.rotationTarget = rotTarget;
        this.startPosition = (Vector2)followTarget.position;
        this.hitEnemies.Clear();
        
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, startPosition + direction * range);
        }
        
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(FireLaser());
        }
    }

    private IEnumerator FireLaser()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            if (followTarget != null)
            {
                startPosition = (Vector2)followTarget.position;
            }
            
            if (rotationTarget != null)
            {
                direction = rotationTarget.up;
            }
            
            Vector2 currentEndPosition = startPosition + direction * range;
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, currentEndPosition);
            }
            
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, range, LayerMask.GetMask("Monster"));
            
            foreach (RaycastHit2D hit in hits)
            {
                IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                if (damageable != null && !hit.collider.CompareTag("Player") && !hitEnemies.Contains(damageable))
                {
                    damageable.TakeDamage(damage);
                    hitEnemies.Add(damageable);
                }
            }
            
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        
        isFiring = false;
        hitEnemies.Clear();
        
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}