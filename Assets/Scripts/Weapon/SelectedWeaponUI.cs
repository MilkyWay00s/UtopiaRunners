using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectedWeaponUI : MonoBehaviour
{
    public TMP_Text weaponNameText;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "7_InGameScene") return;

        WeaponInfo info = WeaponSelectManager.Instance.GetSelectedWeaponInfo();

        if (info != null)
        {
            weaponNameText.text = $"무기 : {info.weaponName}";
            Debug.Log("무기 UI 표시됨: " + info.weaponName);
        }
        else
        {
            Debug.LogWarning("무기 정보 없음 (UI)");
        }
    }
}
