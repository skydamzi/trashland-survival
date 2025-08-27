using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;
    public List<UpgradeCardUI> upgradeCards;
    public float animationDuration = 0.3f;
    public AnimationCurve scaleCurve;

    void Start()
    {
        foreach (var card in upgradeCards)
        {
            card.Init(this);
        }
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades(List<object> upgrades)
    {
        Time.timeScale = 0f;

        foreach(var card in upgradeCards)
        {
            card.gameObject.SetActive(true);
        }

        upgradePanel.SetActive(true);
        StartCoroutine(AnimateShow());

        for (int i = 0; i < upgradeCards.Count; i++)
        {
            if (i < upgrades.Count)
            {
                upgradeCards[i].gameObject.SetActive(true);
                if (upgrades[i] is EquipmentData equipmentData)
                {
                    upgradeCards[i].SetData(equipmentData);
                }
                else if (upgrades[i] is UpgradeData upgradeData)
                {
                    upgradeCards[i].SetData(upgradeData);
                }
            }
            else
            {
                upgradeCards[i].gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator AnimateShow()
    {
        upgradePanel.transform.localScale = Vector3.zero;
        
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / animationDuration;
            float scale = scaleCurve.Evaluate(normalizedTime);
            upgradePanel.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        upgradePanel.transform.localScale = Vector3.one;
    }

    public void Hide()
    {
        StartCoroutine(AnimateHide());
    }

    private IEnumerator AnimateHide()
    {
        float elapsedTime = 0f;
        Vector3 startScale = upgradePanel.transform.localScale;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / animationDuration;
            float scale = scaleCurve.Evaluate(1 - normalizedTime);
            upgradePanel.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
