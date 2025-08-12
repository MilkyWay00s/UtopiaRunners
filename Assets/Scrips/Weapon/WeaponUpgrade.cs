using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    public int weapon1Level = 1;
    public int weapon2Level = 1;

    public void UpgradeWeapon1()
    {
        weapon1Level++;
    }

    public void UpgradeWeapon2()
    {
        weapon2Level++;
    }
}
