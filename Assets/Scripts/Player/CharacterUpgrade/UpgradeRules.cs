using UnityEngine;

public static class UpgradeRules
{
    public static int GetCost(UpgradeType type, int currentLevel)
    {
        int baseCost = type switch
        {
            UpgradeType.MaxHealth => 50,
            UpgradeType.SkillLevel => 60,
            _ => 50
        };

        float growth = 1.35f;
        return Mathf.RoundToInt(baseCost * Mathf.Pow(growth, currentLevel));
    }

    public static int GetMaxHealthBonus(int level) => level * 20;
    public static float GetSkillLevelBonus(int level) => level;
}

