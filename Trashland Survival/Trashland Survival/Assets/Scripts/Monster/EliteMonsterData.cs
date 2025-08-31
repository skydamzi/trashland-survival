using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EliteMonsterData", menuName = "Data/Elite Monster Data")]
public class EliteMonsterData : MonsterData
{
    [Header("엘리트 전용 공격 패턴")]
    public List<AttackPatternSO> attackPatterns;
}