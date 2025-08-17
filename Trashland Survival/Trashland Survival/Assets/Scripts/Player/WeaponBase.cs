using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float damage;
    public float coolDown;
    public abstract void Attack(Transform target);
}