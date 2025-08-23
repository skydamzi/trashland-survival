using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MonsterGroup
{
    public MonsterData monsterData;
    [Range(1, 100)]
    public int count;
}

[CreateAssetMenu(fileName = "New WaveData", menuName = "Data/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("웨이브 정보")]
    public float waveDuration = 60f;
    public float spawnInterval = 1f;

    [Header("스폰할 몬스터 목록")]
    public List<MonsterGroup> monstersInWave;
}
