using UnityEngine;
using System;

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
    public Transform playerTransform;

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

    void Start()
    {
        level = 1;
        maxExp = 100f;
        currentHP = maxHP;
        currentExp = 0f;
    }

    public void GainExp(float expAmount)
    {
        currentExp += expAmount;
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
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Debug.Log("플레이어 사망");
        }
    }
}