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
            _currentHP = value;
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        currentHP = maxHP;
        currentExp = 0f;
        weaponType = "Gun";
        acquiredUpgrades.Clear();
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
