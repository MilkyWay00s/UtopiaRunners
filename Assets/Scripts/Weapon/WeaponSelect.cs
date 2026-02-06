using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public WeaponInfo weaponInfo;
    public GameObject weaponPrefab;

    public TMP_Text weaponNameText1;
    public TMP_Text weaponNameText2;

    public TMP_Text weaponDescriptionText;
    public Image weaponIconImage1;
    public Image weaponIconImage2;

    public void OnClickWeaponButton()
    {
        if (weaponInfo == null) return;

        weaponNameText1.text = weaponInfo.weaponName;
        weaponNameText2.text = weaponInfo.weaponName;
        weaponDescriptionText.text = weaponInfo.weaponDescription;

        if (weaponIconImage1 != null && weaponInfo.weaponIcon != null)
            weaponIconImage1.sprite = weaponInfo.weaponIcon;

        if (weaponIconImage2 != null && weaponInfo.weaponIcon != null)
            weaponIconImage2.sprite = weaponInfo.weaponIcon;
    }
}