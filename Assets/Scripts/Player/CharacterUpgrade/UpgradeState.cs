using UnityEngine;

public enum UpgradeType
{
    MaxHealth,
    SkillLevel
}

public static class UpgradeState
{
    private static string Key(string characterKey, UpgradeType t)
        => $"UPG_{characterKey}_{t}";

    // characterKey 예: "C0", "C1" (캐릭터 인덱스로 만들기)
    public static int GetLevel(string characterKey, UpgradeType t)
        => PlayerPrefs.GetInt(Key(characterKey, t), 0);

    public static int Increase(string characterKey, UpgradeType t)
    {
        int lv = GetLevel(characterKey, t) + 1;
        PlayerPrefs.SetInt(Key(characterKey, t), lv);
        PlayerPrefs.Save();
        return lv;
    }
}
