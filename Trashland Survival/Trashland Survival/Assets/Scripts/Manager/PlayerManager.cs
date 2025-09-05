using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public event Action OnHpChanged { add => playerHealth.OnHpChanged += value; remove => playerHealth.OnHpChanged -= value; }
    public event Action OnExpChanged { add => playerLeveling.OnExpChanged += value; remove => playerLeveling.OnExpChanged -= value; }
    public event Action<int> OnLevelUp { add => playerLeveling.OnLevelUp += value; remove => playerLeveling.OnLevelUp -= value; }

    public int level => playerLeveling.level;
    public float maxHP => playerStats.maxHP;
    public float currentHP => playerHealth.currentHP;
    public float maxExp => playerLeveling.maxExp;
    public float currentExp => playerLeveling.currentExp;
    public float moveSpeed => playerStats.moveSpeed;
    public float attackPower => playerStats.attackPower;
    public float coolDown => playerStats.coolDown;
    public float magnetPower => playerStats.magnetPower;
    public float attackRange => playerStats.attackRange;
    public int gold => playerStats.gold;

    public List<EquipmentData> acquiredUpgrades = new List<EquipmentData>();
    public List<UpgradeData> acquiredStatUpgrades = new List<UpgradeData>();
    public Transform playerTransform;
    public Renderer[] playerRenderers;
    public AudioClip damagedSound;
    
    private PlayerStats playerStats;
    private PlayerHealth playerHealth;
    private PlayerLeveling playerLeveling;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            playerStats = GetComponent<PlayerStats>();
            playerHealth = GetComponent<PlayerHealth>();
            playerLeveling = GetComponent<PlayerLeveling>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        GameEvents.OnNewGameStarted += ResetStats;
        EquipmentManager.OnEquipmentChanged += UpdateStats;
        this.OnLevelUp += HandleLevelUp;
    }

    void OnDisable()
    {
        GameEvents.OnNewGameStarted -= ResetStats;
        EquipmentManager.OnEquipmentChanged -= UpdateStats;
        this.OnLevelUp -= HandleLevelUp;
    }

    void Start()
    {
        PlayerManager.Instance.GainGold(500);
        ResetStats();
    }

    private void HandleLevelUp(int levels)
    {
        UpdateStats();
        Debug.Log($"레벨업! 현재 레벨: {level}");
    }

    public void GainExp(float expAmount)
    {
        playerLeveling.GainExp(expAmount);
    }

    public void TakeDamage(float damage)
    {
        playerHealth.TakeDamage(damage);
    }

    public void GainGold(int amount)
    {
        playerStats.AddGold(amount);
    }
    
    public bool SpendGold(int amount)
    {
        if (playerStats.gold >= amount)
        {
            playerStats.AddGold(-amount);
            return true;
        }
        return false;
    }

    public void UpdateStats()
    {
        acquiredUpgrades.Clear();
        if (EquipmentManager.Instance != null)
        {
            foreach (var list in EquipmentManager.Instance.equippedItems.Values)
            {
                acquiredUpgrades.AddRange(list);
            }
        }

        playerStats.CalculateStats(acquiredStatUpgrades, acquiredUpgrades);
        playerHealth.SetMaxHP(playerStats.maxHP);
    }

    private void ResetStats()
    {
        acquiredUpgrades.Clear();
        acquiredStatUpgrades.Clear();
        
        playerStats.ResetStats();
        playerLeveling.ResetLeveling();
        
        UpdateStats(); 
        playerHealth.ResetHealth(playerStats.maxHP);

        Debug.Log("플레이어 스탯 초기화 완료");
    }
}
