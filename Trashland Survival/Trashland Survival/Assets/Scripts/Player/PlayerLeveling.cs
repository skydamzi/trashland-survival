using UnityEngine;
using System;

public class PlayerLeveling : MonoBehaviour
{
    public event Action OnExpChanged;
    public event Action<int> OnLevelUp;

    [field: SerializeField] public int level { get; private set; }
    [field: SerializeField] public float maxExp { get; private set; }

    [SerializeField] private float _currentExp;
    public float currentExp
    {
        get { return _currentExp; }
        private set
        {
            _currentExp = value;
            OnExpChanged?.Invoke();
        }
    }

    public void ResetLeveling()
    {
        level = 1;
        maxExp = 3f;
        currentExp = 0f;
    }

    public void GainExp(float expAmount)
    {
        currentExp += expAmount;
        LevelUpCheck();
    }

    private void LevelUpCheck()
    {
        if (currentExp < maxExp) return;

        int levelUpCount = 0;
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            maxExp *= 1.5f;
            level++;
            levelUpCount++;
        }

        if (levelUpCount > 0)
        {
            OnLevelUp?.Invoke(levelUpCount);
        }
    }
}
