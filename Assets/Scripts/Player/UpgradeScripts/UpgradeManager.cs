using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeEntry
    {
        public UpgradeData upgradeData;
        public UpgradeBarController upgradeBar;
        public TextMeshProUGUI costText;
        public Button upgradeButton;
    }

    public UpgradeEntry[] upgrades;
    public TextMeshProUGUI playerCoinsText;

    private void Start()
    {
        UpdateUI();
    }

    public void Upgrade(int index)
    {
        if (index < 0 || index >= upgrades.Length) return;

        var entry = upgrades[index];
        var data = entry.upgradeData;

        int cost = data.GetCost();

        if (data.currentLevel >= data.maxLevel)
        {
            Debug.Log($"{data.upgradeName}은(는) 이미 최대 레벨입니다.");
            return;
        }

        if (CoinManager.Instance.SpendCoins(cost))
        {
            data.currentLevel++;

            ApplyUpgradeEffect(data);

            entry.upgradeBar.UpdateBarVisual();
            UpdateUI();
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    void ApplyUpgradeEffect(UpgradeData data)
    {
        switch (data.upgradeName)
        {
            case "AttackUpgrade":
                PlayerStats.Instance.UpgradeAttack(data.effectValuePerLevel);
                break;
            case "HealthUpgrade":
                PlayerStats.Instance.UpgradeHealth(data.effectValuePerLevel);
                break;
            case "ArmorUpgrade":
                PlayerStats.Instance.UpgradeArmor(data.effectValuePerLevel);
                break;
        }
    }

    void UpdateUI()
    {
        foreach (var entry in upgrades)
        {
            int cost = entry.upgradeData.GetCost();

            if (cost < 0)
            {
                entry.costText.text = "MAX";
                entry.upgradeButton.interactable = false;
            }
            else
            {
                entry.costText.text = cost.ToString();
                entry.upgradeButton.interactable = true;
            }
        }
    }
}
