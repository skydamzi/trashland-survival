using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GachaEntry
{
    public EquipmentData equipment;
    [Tooltip("뽑힐 확률 가중치")]
    public float weight = 1f;
}

[CreateAssetMenu(fileName = "NewGachaPool", menuName = "Data/Gacha Pool")]
public class GachaPoolData : ScriptableObject
{
    public string poolName;
    public int costGold = 100;
    public List<GachaEntry> items;
}