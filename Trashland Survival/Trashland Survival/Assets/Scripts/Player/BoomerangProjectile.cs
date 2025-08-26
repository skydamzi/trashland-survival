using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour, IDamageDealer
{
    private Boomerang ownerWeapon;
    private Transform player;
    private float damage;
    private float throwDistance;
    private float orbitSpeed = 200f;
    private float returnSpeed = 10f;
    private float rotationSpeed = 1440f;

    private enum State { Throwing, Orbiting, Returning, Inactive }
    private State currentState;

    private Vector3 throwDirection;
    private Vector3 orbitStartPosition;
    private float orbitAngle;
    private readonly HashSet<IDamageable> hitEnemies = new HashSet<IDamageable>();

    void OnEnable()
    {
        hitEnemies.Clear();
        currentState = State.Inactive;
        orbitAngle = 0f;
    }

    public void Initialize(Boomerang owner, Transform initialTarget, float dmg, float range)
    {
        ownerWeapon = owner;
        player = owner.transform.root;
        damage = dmg;
        throwDistance = range;
        
        if(initialTarget != null)
        {
            throwDirection = (initialTarget.position - player.position).normalized;
        }
        else
        {
            throwDirection = owner.transform.right;
        }
        
        currentState = State.Throwing;
    }

    void Update()
    {
        if (currentState == State.Inactive) return;

        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        switch (currentState)
        {
            case State.Throwing:
                transform.position += throwDirection * returnSpeed * Time.deltaTime;
                if (Vector3.Distance(player.position, transform.position) >= throwDistance)
                {
                    currentState = State.Orbiting;
                    orbitStartPosition = transform.position - player.position;
                }
                break;

            case State.Orbiting:
                orbitAngle += orbitSpeed * Time.deltaTime;
                Vector3 offset = Quaternion.Euler(0, 0, orbitAngle) * orbitStartPosition;
                transform.position = player.position + offset;
                if (orbitAngle >= 360f)
                {
                    currentState = State.Returning;
                }
                break;

            case State.Returning:
                transform.position = Vector3.MoveTowards(transform.position, player.position, returnSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, player.position) < 0.5f)
                {
                    ownerWeapon.OnBoomerangReturned(transform.rotation);
                    PoolManager.Instance.ReturnToPool(gameObject);
                    currentState = State.Inactive;
                }
                break;
        }
    }

        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root == player) return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null && !hitEnemies.Contains(damageable))
        {
            damageable.TakeDamage(GetDamage());
            hitEnemies.Add(damageable);
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public GameObject GetOwner()
    {
        return player.gameObject;
    }
}