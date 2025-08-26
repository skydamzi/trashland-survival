using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public event Action OnHpChanged, OnExpChanged, OnLevelUp;

    public int level;
    public float maxHP;
    private float _currentHP;
    public float currentHP
    {
        get { return _currentHP; }
        set
        {
            _currentHP = Mathf.Clamp(value, 0, maxHP);
            OnHpChanged?.Invoke();
        }
    }
    public float maxExp;
    private float _currentExp;
    public float currentExp
    {
        get { return _currentExp; }
        set
        {
            _currentExp = value;
            OnExpChanged?.Invoke();
        }
    }
    public float moveSpeed, attackPower, coolDown;
    public float magnetPower;
    public float attackRange;
    public string weaponType;
    public List<EquipmentData> acquiredUpgrades = new List<EquipmentData>();

    public Transform playerTransform;
    public Renderer[] playerRenderers;
    private float blinkDuration = 1f;
    private float blinkInterval = 0.1f;
    private bool isInvincible = false;

    private float baseMaxHP;
    private float baseMoveSpeed;
    private float baseAttackPower;
    private float baseCoolDown;
    private float baseMagnetPower;
    private float baseAttackRange;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateStats()
    {
        float totalHealthBonus = 0;
        float totalMoveSpeedBonus = 0;
        float totalAttackPowerBonus = 0;
        float totalCooldownReduction = 0;
        float totalMagnetBonus = 0;
        float totalAttackRangeBonus = 0;

        foreach (var upgrade in acquiredUpgrades)
        {
            totalHealthBonus += upgrade.healthBonus;
            totalMoveSpeedBonus += upgrade.moveSpeedBonus;
            totalAttackPowerBonus += upgrade.attackPowerBonus;
            totalCooldownReduction += upgrade.cooldownReduction;
            totalMagnetBonus += upgrade.magnetBonus;
            totalAttackRangeBonus += upgrade.attackPowerBonus;
        }

        maxHP = baseMaxHP + totalHealthBonus;
        moveSpeed = baseMoveSpeed * (1 + totalMoveSpeedBonus / 100f);
        attackPower = baseAttackPower * (1 + totalAttackPowerBonus / 100f);
        coolDown = baseCoolDown * (1 - totalCooldownReduction / 100f);
        magnetPower = baseMagnetPower * (1 + totalMagnetBonus / 100f);
        attackRange = baseAttackRange * (1 + totalAttackRangeBonus / 100f);
    }

    void OnEnable()
    {
        GameEvents.OnNewGameStarted += ResetStats;
    }

    void OnDisable()
    {
        GameEvents.OnNewGameStarted -= ResetStats;
    }

    void Start()
    {
        ResetStats();
    }

    private void ResetStats()
    {
        level = 1;
        maxExp = 3f;
        acquiredUpgrades.Clear();
        baseMaxHP = 100f;
        baseMoveSpeed = 3f;
        baseAttackPower = 10f;
        baseCoolDown = 1f;
        baseMagnetPower = 1f;
        baseAttackRange = 3f;
        UpdateStats(); 
        currentHP = maxHP;
        currentExp = 0f;
        weaponType = "Gun";
        Debug.Log("플레이어 스탯 초기화 완료");
    }

    public void GainExp(float expAmount)
    {
        currentExp += expAmount;
        LevelUpCheck();
    }

    void LevelUpCheck()
    {
        if (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            maxExp *= 1.5f;
            level++;
            
            baseMaxHP += 10f;
            baseMoveSpeed += 1f;
            baseAttackPower += 5f;

            UpdateStats();
            Debug.Log($"레벨업 현재 레벨: {level}");
            OnLevelUp?.Invoke();
        }
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHP -= damage;
        StartCoroutine(InvincibilityEffect());
        if (currentHP <= 0)
        {
            currentHP = 0;
            Debug.Log("플레이어 사망");
        }
    }
    private IEnumerator InvincibilityEffect()
    {
        isInvincible = true;

        if (playerRenderers.Length == 0)
        {
            yield break;
        }

        float timer = 0f;
        while (timer < blinkDuration)
        {
            foreach (Renderer renderer in playerRenderers)
            {
                if (renderer != null) renderer.enabled = !renderer.enabled;
            }
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer != null) renderer.enabled = true;
        }

        isInvincible = false;
    }
}