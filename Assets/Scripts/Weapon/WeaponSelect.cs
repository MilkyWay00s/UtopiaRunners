using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public static GameObject selectedWeapon;
    public GameObject weaponSelect;
    private static WeaponSelect instance;
    public GameObject weapon1;
    public GameObject weapon2;
    public Transform slotSelecter;
    public Transform slot1;
    public Transform slot2;

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
        if (slotSelecter == null || slot1 == null || slot2 == null)
            return;

        if (slotSelecter.position == slot1.position)
        {
            weaponSelect = weapon1;
        }
        else if (slotSelecter.position == slot2.position)
        {
            weaponSelect = weapon2;
        }

        selectedWeapon = weaponSelect; // 무기를 선택하여 정적으로 저장
    }
}
