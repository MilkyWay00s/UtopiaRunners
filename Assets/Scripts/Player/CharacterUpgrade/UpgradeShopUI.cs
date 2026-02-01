using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeShopUI : MonoBehaviour
{
    [Header("Selected Character")]
    [SerializeField] private string currentCharacterKey = "C0"; // 기본 선택 캐릭터

    [Header("Coin UI")]
    public TMP_Text coinText;

    [Header("Max Health UI")]
    public TMP_Text hpLevelText;
    public TMP_Text hpCostText;
    public Button hpUpgradeButton;

    [Header("Weapon Damage UI")]
    public TMP_Text dmgLevelText;
    public TMP_Text dmgCostText;
    public Button dmgUpgradeButton;

    [Header("Attack Speed UI")]
    public TMP_Text aspdLevelText;
    public TMP_Text aspdCostText;
    public Button aspdUpgradeButton;

    void OnEnable()
    {
        if (hpUpgradeButton) hpUpgradeButton.onClick.AddListener(OnClickHp);
        if (dmgUpgradeButton) dmgUpgradeButton.onClick.AddListener(OnClickDmg);
        if (aspdUpgradeButton) aspdUpgradeButton.onClick.AddListener(OnClickAspd);

        RefreshAll();
    }

    void OnDisable()
    {
        if (hpUpgradeButton) hpUpgradeButton.onClick.RemoveListener(OnClickHp);
        if (dmgUpgradeButton) dmgUpgradeButton.onClick.RemoveListener(OnClickDmg);
        if (aspdUpgradeButton) aspdUpgradeButton.onClick.RemoveListener(OnClickAspd);
    }

    //  캐릭터 선택 버튼이 호출할 API
    public void SelectCharacterByIndex(int characterIndex)
    {
        currentCharacterKey = $"C{characterIndex}";
        RefreshAll();
    }

    public void SelectCharacterKey(string key)
    {
        currentCharacterKey = key;
        RefreshAll();
    }

    void OnClickHp() => TryUpgrade(UpgradeType.MaxHealth);
    void OnClickDmg() => TryUpgrade(UpgradeType.WeaponDamage);
    void OnClickAspd() => TryUpgrade(UpgradeType.WeaponAttackSpeed);

    void TryUpgrade(UpgradeType type)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[UpgradeShopUI] GameManager.Instance가 없습니다.");
            return;
        }

        int level = UpgradeState.GetLevel(currentCharacterKey, type);
        int cost = UpgradeRules.GetCost(type, level);

        bool ok = GameManager.Instance.TrySpendCoin(cost); // GameManager에 이 함수 필요
        if (!ok)
        {
            Debug.Log("코인이 부족합니다.");
            return;
        }

        UpgradeState.Increase(currentCharacterKey, type);
        RefreshAll();
    }

    void RefreshAll()
    {
        int coins = (GameManager.Instance != null) ? GameManager.Instance.coin : 0;
        if (coinText) coinText.text = $"Coins: {coins}";

        RefreshSlot(UpgradeType.MaxHealth, hpLevelText, hpCostText, hpUpgradeButton);
        RefreshSlot(UpgradeType.WeaponDamage, dmgLevelText, dmgCostText, dmgUpgradeButton);
        RefreshSlot(UpgradeType.WeaponAttackSpeed, aspdLevelText, aspdCostText, aspdUpgradeButton);
    }

    void RefreshSlot(UpgradeType type, TMP_Text levelText, TMP_Text costText, Button btn)
    {
        int level = UpgradeState.GetLevel(currentCharacterKey, type);
        int cost = UpgradeRules.GetCost(type, level);

        if (levelText) levelText.text = $"Lv. {level}";
        if (costText) costText.text = $"Cost: {cost}";

        if (btn && GameManager.Instance != null)
            btn.interactable = (GameManager.Instance.coin >= cost);
    }
}
