using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    private GameObject player;
    private GameObject weapon = WeaponSelect.selectedWeapon; // 전투 준비 Scene에서 장착된 무기를 받아옴

    private void Start()
    {
        if (WeaponSelect.selectedWeapon != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            weapon = Instantiate(weapon, player.transform.position, Quaternion.identity); // 무기를 Player 위치에 소환
        }
    }

    private void Update()
    {
        if (weapon != null && player != null)
        {
            weapon.transform.position = player.transform.position;
        }
    }
}
