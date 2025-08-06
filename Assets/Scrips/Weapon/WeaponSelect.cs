using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public static GameObject selectedWeapon;
    public GameObject weaponSelect;
    private static WeaponSelect instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        selectedWeapon = weaponSelect; // ���⸦ �����Ͽ� �������� ����
    }
}
