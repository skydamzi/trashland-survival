using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerangBullet : MonoBehaviour
{
    private float damage;
    private float range;
    private float lifeTime;
    private Transform target;
    private Vector2 startPosition;
    private bool isReturning = false;
    private Boomerang boomerang;
    private Rigidbody2D rb;
    private float speed;
    private HashSet<IDamageable> hitEnemies = new HashSet<IDamageable>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Boomerang boo, Transform target, float damage, float range, float lifeTime)
    {
        this.boomerang = boo;
        this.target = target;
        this.damage = damage;
        this.range = range;
        this.lifeTime = lifeTime;
        this.startPosition = transform.position;
        this.isReturning = false;
        this.speed = boo.boomerangSpeed;
        hitEnemies.Clear();

        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }

        StartCoroutine(TravelAndReturn());
    }

    void Update()
    {
        if (isReturning)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
        }
    }

    private IEnumerator TravelAndReturn()
    {
        Vector2 initialDirection = Vector2.zero;
        if (target != null)
        {
            initialDirection = ((Vector2)target.position - startPosition).normalized;
        }
        else
        {
            initialDirection = rb.linearVelocity.normalized;
        }
        
        float straightDistance = range;
        
        while (!isReturning && Vector2.Distance(startPosition, transform.position) < straightDistance)
        {
            rb.linearVelocity = initialDirection * speed;
            yield return null;
        }

        isReturning = true;
        
        yield return new WaitForSeconds(lifeTime);
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isReturning)
        {
            OnReturnToPlayer();
            return;
        }

        if (!isReturning)
        {
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null && !other.CompareTag("Player") && !hitEnemies.Contains(damageable))
            {
                damageable.TakeDamage(damage);
                hitEnemies.Add(damageable);
            }
        }
    }

    private void OnReturnToPlayer()
    {
        PoolManager.Instance.ReturnToPool(gameObject);
        if (boomerang != null)
        {
            boomerang.OnBoomerangReturned();
        }
    }
}