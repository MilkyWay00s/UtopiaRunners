using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public GameObject player;
    private GameObject weapon = WeaponSelect.selectedWeapon; // ���� �غ� Scene���� ������ ���⸦ �޾ƿ�

    private void Start()
    {
        if (WeaponSelect.selectedWeapon != null)
        {
            weapon = Instantiate(weapon, player.transform.position, Quaternion.identity); // ���⸦ Player ��ġ�� ��ȯ
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
