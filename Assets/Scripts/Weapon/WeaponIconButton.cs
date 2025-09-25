using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIconButton : MonoBehaviour
{
    public WeaponInfo weaponInfo;
    public GameObject weaponPrefab;

    public WeaponSelectPanelScript selectPanel;

    public void OnClickIcon()
    {
        if (selectPanel == null) return;

        selectPanel.SetWeapon(weaponInfo, weaponPrefab);
    }
}
