using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    public WeaponInfo weaponInfo;
    public GameObject weaponPrefab;

    public TMP_Text weaponNameText;
    public TMP_Text weaponDescriptionText;
    public Image weaponIconImage;

    public void OnClickWeaponButton()
    {
        if (weaponInfo == null) return;

        weaponNameText.text = weaponInfo.weaponName;
        weaponDescriptionText.text = weaponInfo.weaponDescription;

        if (weaponIconImage != null && weaponInfo.weaponIcon != null)
            weaponIconImage.sprite = weaponInfo.weaponIcon;
    }
}