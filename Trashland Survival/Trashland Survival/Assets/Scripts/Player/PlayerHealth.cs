using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnHpChanged;

    [field: SerializeField] public float maxHP { get; private set; }
    [SerializeField] private float _currentHP;
    public float currentHP
    {
        get { return _currentHP; }
        private set
        {
            _currentHP = Mathf.Clamp(value, 0, maxHP);
            OnHpChanged?.Invoke();
        }
    }

    private bool isInvincible = false;
    private float blinkDuration = 1f;
    private float blinkInterval = 0.1f;

    private PlayerManager playerManager;
    private Renderer[] playerRenderers => playerManager.playerRenderers;
    private AudioClip damagedSound => playerManager.damagedSound;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void ResetHealth(float newMaxHP)
    {
        SetMaxHP(newMaxHP);
        currentHP = maxHP;
        isInvincible = false;
    }

    public void SetMaxHP(float newMaxHP)
    {
        float hpDiff = newMaxHP - maxHP;
        maxHP = newMaxHP;
        if (hpDiff > 0)
        {
            currentHP += hpDiff;
        }
        else
        {
            currentHP = Mathf.Min(currentHP, maxHP);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;
        if (SoundManager.Instance != null && damagedSound != null)
        {
            SoundManager.Instance.PlaySFX(damagedSound);
        }
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

        if (playerRenderers == null || playerRenderers.Length == 0)
        {
            isInvincible = false;
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