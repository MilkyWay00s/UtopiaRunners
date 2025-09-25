using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponInfo", menuName = "Weapon/Weapon Info")]
public class WeaponInfo : ScriptableObject
{
    public string weaponName;
    [TextArea(3, 6)]
    public string weaponDescription;
    public Sprite weaponIcon;
}
