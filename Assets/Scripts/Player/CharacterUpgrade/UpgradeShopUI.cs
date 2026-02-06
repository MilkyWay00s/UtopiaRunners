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

    [Header("Skill Level UI")]
    public TMP_Text sklvLevelText;
    public TMP_Text sklvCostText;
    public Button sklvUpgradeButton;


    void OnEnable()
    {
        if (hpUpgradeButton) hpUpgradeButton.onClick.AddListener(OnClickHp);
        if (sklvUpgradeButton) sklvUpgradeButton.onClick.AddListener(OnClickSklv);

        RefreshAll();
    }

    void OnDisable()
    {
        if (hpUpgradeButton) hpUpgradeButton.onClick.RemoveListener(OnClickHp);
        if (sklvUpgradeButton) sklvUpgradeButton.onClick.RemoveListener(OnClickSklv);
        
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
    void OnClickSklv() => TryUpgrade(UpgradeType.SkillLevel);
    

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
        RefreshSlot(UpgradeType.SkillLevel, sklvLevelText, sklvCostText, sklvUpgradeButton);
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
