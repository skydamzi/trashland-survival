using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;
    public List<UpgradeCardUI> upgradeCards;

    void Start()
    {
        foreach (var card in upgradeCards)
        {
            card.Init(this);
        }
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades(List<EquipmentData> upgrades)
    {
        Time.timeScale = 0f;
        upgradePanel.SetActive(true);

        for (int i = 0; i < upgradeCards.Count; i++)
        {
            if (i < upgrades.Count)
            {
                upgradeCards[i].gameObject.SetActive(true);
                upgradeCards[i].SetData(upgrades[i]);
            }
            else
            {
                upgradeCards[i].gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
