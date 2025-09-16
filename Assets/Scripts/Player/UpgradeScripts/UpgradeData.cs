using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Game/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public int baseCost;
    public float costMultiplier = 1.5f;
    public int currentLevel = 0;
    public int maxLevel = 10;
    public float effectValuePerLevel = 5f;

    public int GetCost()
    {
        if (currentLevel >= maxLevel)
            return -1; 

        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));
    }

    public float GetEffectValue() => currentLevel * effectValuePerLevel;
}
