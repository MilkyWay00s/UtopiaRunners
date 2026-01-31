using UnityEngine;

public static class UpgradeRules
{
    public static int GetCost(UpgradeType type, int currentLevel)
    {
        int baseCost = type switch
        {
            UpgradeType.MaxHealth => 50,
            UpgradeType.WeaponDamage => 60,
            UpgradeType.WeaponAttackSpeed => 70,
            _ => 50
        };

        float growth = 1.35f;
        return Mathf.RoundToInt(baseCost * Mathf.Pow(growth, currentLevel));
    }

    public static int GetMaxHealthBonus(int level) => level * 20;
    public static float GetWeaponDamageBonus(int level) => level * 0.15f;
    public static float GetAttackSpeedBonus(int level) => level * 0.10f;
}
