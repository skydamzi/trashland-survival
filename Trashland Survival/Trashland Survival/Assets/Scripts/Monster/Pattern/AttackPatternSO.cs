using UnityEngine;

public abstract class AttackPatternSO : ScriptableObject
{
    public float cooldown;

    public abstract void Execute(Monster monster);
}
