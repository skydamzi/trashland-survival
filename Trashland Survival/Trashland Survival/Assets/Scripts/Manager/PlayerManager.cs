using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public event Action OnHpChanged, OnExpChanged, OnLevelUp;

    // Initial Stats (set in Inspector)
    private float initialMaxHP, initialMoveSpeed, initialAttackPower, initialCoolDown, initialMagnetPower, initialAttackRange;

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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            StoreInitialStats();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void StoreInitialStats()
    {
        initialMaxHP = maxHP;
        initialMoveSpeed = moveSpeed;
        initialAttackPower = attackPower;
        initialCoolDown = coolDown;
        initialMagnetPower = magnetPower;
        initialAttackRange = attackRange;
    }

    public void UpdateStats()
    {
        maxHP = initialMaxHP;
        moveSpeed = initialMoveSpeed;
        attackPower = initialAttackPower;
        coolDown = initialCoolDown;
        magnetPower = initialMagnetPower;
        attackRange = initialAttackRange;

        float totalHealthPercent = 0;
        float totalMoveSpeedPercent = 0;
        float totalAttackPowerPercent = 0;
        float totalCooldownReductionPercent = 0;
        float totalMagnetPercent = 0;
        float totalAttackRangePercent = 0;

        foreach (var upgrade in acquiredUpgrades)
        {
            totalHealthPercent += upgrade.healthBonus;
            totalMoveSpeedPercent += upgrade.moveSpeedBonus;
            totalAttackPowerPercent += upgrade.attackPowerBonus;
            totalCooldownReductionPercent += upgrade.cooldownReduction;
            totalMagnetPercent += upgrade.magnetBonus;
            totalAttackRangePercent += upgrade.attackRangeBonus;
        }


        maxHP += initialMaxHP * (totalHealthPercent / 100f);
        moveSpeed += initialMoveSpeed * (totalMoveSpeedPercent / 100f);
        attackPower += initialAttackPower * (totalAttackPowerPercent / 100f);
        coolDown -= initialCoolDown * (totalCooldownReductionPercent / 100f);
        magnetPower += initialMagnetPower * (totalMagnetPercent / 100f);
        attackRange += initialAttackRange * (totalAttackRangePercent / 100f);
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
