using System.Collections.Generic;
using UnityEngine;

public enum EliteAttackPattern
{
    Charge,
    RangedAttack,
    AreaOfEffect,
}

[CreateAssetMenu(fileName = "New EliteMonsterData", menuName = "Data/Elite Monster Data")]
public class EliteMonsterData : MonsterData
{
    [Header("엘리트 전용")]
    public List<EliteAttackPattern> attackPatterns;
}
