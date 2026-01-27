#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

public static class DebugMenu
{
    private const string MENU_ROOT = "Tools/Chat/";

    // -----------------------------
    // 현재 스테이지 채팅 리셋
    // -----------------------------
    [MenuItem(MENU_ROOT + "Reset Current Stage Chat")]
    public static void ResetCurrentStageChat()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[ChatDebug] GameManager.Instance is null");
            return;
        }

        string stageKey = GameManager.Instance.SelectedStageId.ToString();

        string key = $"ChatSeen_{stageKey}";

        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"[ChatDebug] Reset chat seen: {key}");
        }
        else
        {
            Debug.Log($"[ChatDebug] No chat seen key found: {key}");
        }
    }

    // -----------------------------
    // 모든 스테이지 채팅 리셋
    // -----------------------------
    [MenuItem(MENU_ROOT + "Reset ALL Stage Chats")]
    public static void ResetAllStageChats()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[ChatDebug] GameManager.Instance is null");
            return;
        }

        foreach (StageName stage in Enum.GetValues(typeof(StageName)))
        {
            string key = $"ChatSeen_{stage}";
            if (PlayerPrefs.HasKey(key))
                PlayerPrefs.DeleteKey(key);
        }

        PlayerPrefs.Save();
        Debug.Log("[ChatDebug] Reset ALL chat seen flags");
    }
}
#endif
