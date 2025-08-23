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
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    public void Initialize(Boomerang owner, Transform initialTarget, float dmg, float range)
    {
        ownerWeapon = owner;
        player = owner.transform.root;
        damage = dmg;
        throwDistance = range;
        throwDirection = (initialTarget.position - player.position).normalized;
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
                    orbitAngle = 0f;
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
                    Destroy(gameObject);
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hitEnemies.Contains(other)) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (other.transform.root != player)
            {
                damageable.TakeDamage(GetDamage());
                hitEnemies.Add(other);
            }
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