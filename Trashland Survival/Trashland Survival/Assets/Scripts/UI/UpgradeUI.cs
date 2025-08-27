using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;
    public List<UpgradeCardUI> upgradeCards;
    public float animationDuration = 0.3f;
    public AnimationCurve scaleCurve;

    private Dictionary<UpgradeCardUI, Vector3> initialCardPositions = new Dictionary<UpgradeCardUI, Vector3>();

    void Start()
    {
        foreach (var card in upgradeCards)
        {
            card.Init(this);
            initialCardPositions.Add(card, card.transform.localPosition);
        }
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades(List<object> upgrades)
    {
        Time.timeScale = 0f;
        
        upgradePanel.SetActive(true);

        for (int i = 0; i < upgradeCards.Count; i++)
        {
            upgradeCards[i].gameObject.SetActive(true);
            upgradeCards[i].transform.localPosition = initialCardPositions[upgradeCards[i]];
            upgradeCards[i].transform.localScale = Vector3.one;

            if (i < upgrades.Count)
            {
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

        StartCoroutine(AnimateShow());
    }

    private IEnumerator AnimateShow()
    {
        upgradePanel.transform.localScale = Vector3.one;

        List<UpgradeCardUI> activeCards = new List<UpgradeCardUI>();
        foreach (var card in upgradeCards)
        {
            if (card.gameObject.activeSelf)
            {
                activeCards.Add(card);
                card.transform.localScale = Vector3.zero;
            }
        }

        for (int i = 0; i < activeCards.Count; i++)
        {
            StartCoroutine(AnimateCard(activeCards[i]));
            if (i < activeCards.Count - 1)
            {
                yield return new WaitForSecondsRealtime(animationDuration * 0.5f);
            }
        }
    }

    private IEnumerator AnimateCard(UpgradeCardUI card)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / animationDuration;
            float scale = scaleCurve.Evaluate(normalizedTime);
            card.transform.localScale = Vector3.one * scale;
            yield return null;
        }
        card.transform.localScale = Vector3.one;
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

    public void OnCardSelected(UpgradeCardUI selectedCard)
    {
        Time.timeScale = 1f;

        StartCoroutine(AnimateSelectedCard(selectedCard));

        foreach (var card in upgradeCards)
        {
            if (card != selectedCard)
            {
                StartCoroutine(AnimateUnselectedCard(card));
            }
        }

        StartCoroutine(DeactivatePanelAfterDelay(0.3f));
    }

    private IEnumerator AnimateSelectedCard(UpgradeCardUI card)
    {
        Vector3 originalScale = card.transform.localScale;
        Vector3 targetScale = originalScale * 1.1f;

        Vector3 originalPosition = card.transform.localPosition;
        Vector3 targetPosition = originalPosition + new Vector3(0, 50f, 0);

        float elapsedTime = 0f;
        float moveScaleDuration = 0.2f;
        AnimationCurve curve = scaleCurve;

        while (elapsedTime < moveScaleDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / moveScaleDuration;

            card.transform.localScale = Vector3.Lerp(originalScale, targetScale, curve.Evaluate(normalizedTime));
            card.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, curve.Evaluate(normalizedTime));

            yield return null;
        }
        card.transform.localScale = targetScale;
        card.transform.localPosition = targetPosition;
    }

    private IEnumerator AnimateUnselectedCard(UpgradeCardUI card)
    {
        Vector3 startPosition = card.transform.position;
        Vector3 targetPosition = upgradePanel.transform.position;

        Vector3 startScale = card.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        float elapsedTime = 0f;
        float duration = 0.3f;
        AnimationCurve curve = scaleCurve;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = elapsedTime / duration;

            card.transform.position = Vector3.Lerp(startPosition, targetPosition, curve.Evaluate(normalizedTime));
            
            card.transform.localScale = Vector3.Lerp(startScale, targetScale, curve.Evaluate(normalizedTime));
            yield return null;
        }
        card.transform.localScale = targetScale;
        card.gameObject.SetActive(false);
    }

    private IEnumerator DeactivatePanelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(AnimateHide());
    }
}
