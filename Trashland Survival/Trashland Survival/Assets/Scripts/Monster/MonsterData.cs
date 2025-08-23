using UnityEngine;

[CreateAssetMenu(fileName = "New MonsterData", menuName = "Data/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public string monsterName;
    public GameObject prefab;
    public GameObject expGemPrefab;

    [Header("스탯")]
    public float health;
    public float attackPower;
    public float moveSpeed;
    public int expAmount;
}