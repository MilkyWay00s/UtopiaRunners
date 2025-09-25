using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSelectPanelScript : MonoBehaviour
{
    private WeaponInfo weaponInfo;
    private GameObject weaponPrefab;

    public void SetWeapon(WeaponInfo info, GameObject prefab)
    {
        weaponInfo = info;
        weaponPrefab = prefab;
    }

    public void OnSelectWeaponButton()
    {
        if (weaponInfo == null || weaponPrefab == null)
        {
            Debug.LogWarning("선택된 무기가 없습니다");
            return;
        }

        WeaponSelectManager.Instance.SelectWeapon(weaponInfo, weaponPrefab);

        SceneManager.LoadScene("7_InGameScene");
    }
}