using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public GameObject autoAttack;
    public float attackSpeed = 1f; // 공격 속도

    private void Start()
    {
        StartCoroutine(AutoAttack());
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            Instantiate(autoAttack, transform.position, transform.rotation); // 기본 공격을 일정 시간마다 소환
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}
