using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public GameObject autoAttack;
    public float attackSpeed = 1f; // ���� �ӵ�

    private void Start()
    {
        StartCoroutine(AutoAttack());
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            Instantiate(autoAttack, transform.position, transform.rotation); // �⺻ ������ ���� �ð����� ��ȯ
            yield return new WaitForSeconds(attackSpeed);
        }
    }
}
