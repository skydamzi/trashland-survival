using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public event Action OnHpChanged, OnExpChanged;
    public event Action<int> OnLevelUp;

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
    public List<UpgradeData> acquiredStatUpgrades = new List<UpgradeData>();

    public Transform playerTransform;
    public Renderer[] playerRenderers;
    private PlayerAttackController playerAttackController;
    private float blinkDuration = 1f;
    private float blinkInterval = 0.1f;
    private bool isInvincible = false;

    private float baseMaxHP;
    private float baseMoveSpeed;
    private float baseAttackPower;
    private float baseCoolDown;
    private float baseMagnetPower;
    private float baseAttackRange;

    public AudioClip damagedSound;

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
        playerAttackController = FindFirstObjectByType<PlayerAttackController>();
    }

    public void UpdateStats()
    {
        float augmentHealthPercent = 0;
        float augmentMoveSpeedPercent = 0;
        float augmentAttackPowerPercent = 0;
        float augmentCooldownPercent = 0;
        float augmentMagnetPercent = 0;
        float augmentAttackRangePercent = 0;

        foreach (var statUpgrade in acquiredStatUpgrades)
        {
            switch (statUpgrade.upgradeType)
            {
                case UpgradeType.Health:
                    augmentHealthPercent += statUpgrade.value;
                    break;
                case UpgradeType.AttackPower:
                    augmentAttackPowerPercent += statUpgrade.value;
                    break;
                case UpgradeType.MoveSpeed:
                    augmentMoveSpeedPercent += statUpgrade.value;
                    break;
                case UpgradeType.CooldownReduction:
                    augmentCooldownPercent += statUpgrade.value;
                    break;
                case UpgradeType.Magnet:
                    augmentMagnetPercent += statUpgrade.value;
                    break;
                case UpgradeType.AttackRange:
                    augmentAttackRangePercent += statUpgrade.value;
                    break;
            }
        }

        float augmentedMaxHP = baseMaxHP * (1 + augmentHealthPercent / 100f);
        float augmentedMoveSpeed = baseMoveSpeed * (1 + augmentMoveSpeedPercent / 100f);
        float augmentedAttackPower = baseAttackPower * (1 + augmentAttackPowerPercent / 100f);
        float augmentedCoolDown = baseCoolDown * (1 - augmentCooldownPercent / 100f);
        float augmentedMagnetPower = baseMagnetPower * (1 + augmentMagnetPercent / 100f);
        float augmentedAttackRange = baseAttackRange * (1 + augmentAttackRangePercent / 100f);

        acquiredUpgrades.Clear();
        if (EquipmentManager.Instance != null)
        {
            foreach (var list in EquipmentManager.Instance.equippedItems.Values)
            {
                acquiredUpgrades.AddRange(list);
            }
        }

        float equipHealthPercent = 0;
        float equipMoveSpeedPercent = 0;
        float equipAttackPowerPercent = 0;
        float equipCooldownPercent = 0;
        float equipMagnetPercent = 0;
        float equipAttackRangePercent = 0;

        foreach (var equip in acquiredUpgrades)
        {
            equipHealthPercent += equip.healthBonus;
            equipMoveSpeedPercent += equip.moveSpeedBonus;
            equipAttackPowerPercent += equip.attackPowerBonus;
            equipCooldownPercent += equip.cooldownReduction;
            equipMagnetPercent += equip.magnetBonus;
            equipAttackRangePercent += equip.attackRangeBonus;
        }

        maxHP = augmentedMaxHP * (1 + equipHealthPercent / 100f);
        moveSpeed = augmentedMoveSpeed * (1 + equipMoveSpeedPercent / 100f);
        attackPower = augmentedAttackPower * (1 + equipAttackPowerPercent / 100f);
        coolDown = augmentedCoolDown * (1 - equipCooldownPercent / 100f);
        magnetPower = augmentedMagnetPower * (1 + equipMagnetPercent / 100f);
        attackRange = augmentedAttackRange * (1 + equipAttackRangePercent / 100f);

        OnHpChanged?.Invoke();
    }

    void OnEnable()
    {
        GameEvents.OnNewGameStarted += ResetStats;
        GameEvents.OnWeaponSwapRequested.AddListener(HandleWeaponSwapRequest);
        EquipmentManager.OnEquipmentChanged += UpdateStats;
    }

    void OnDisable()
    {
        GameEvents.OnNewGameStarted -= ResetStats;
        GameEvents.OnWeaponSwapRequested.RemoveListener(HandleWeaponSwapRequest);
        EquipmentManager.OnEquipmentChanged -= UpdateStats;
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
        acquiredStatUpgrades.Clear();
        baseMaxHP = 100f;
        baseMoveSpeed = 2f;
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
        if (currentExp < maxExp) return;

        int levelUpCount = 0;
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            maxExp *= 1.5f;
            level++;
            levelUpCount++;
            UpdateStats();
            Debug.Log($"레벨업 현재 레벨: {level}");
            OnExpChanged?.Invoke();
        }

        if (levelUpCount > 0)
        {
            OnLevelUp?.Invoke(levelUpCount);
        }
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible) return;
        SoundManager.Instance.PlaySFX(damagedSound);
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

    private void HandleWeaponSwapRequest()
    {
        string currentWeapon = weaponType;
        string nextWeapon = "";

        switch (currentWeapon)
        {
            case "Gun":
                nextWeapon = "Punch";
                break;
            case "Punch":
                nextWeapon = "Boomerang";
                break;
            case "Boomerang":
                nextWeapon = "Gun";
                break;
            default:
                nextWeapon = "Gun";
                break;
        }

        weaponType = nextWeapon;

        if (playerAttackController != null)
        {
            playerAttackController.UpdateWeapon();
        }
    }
}