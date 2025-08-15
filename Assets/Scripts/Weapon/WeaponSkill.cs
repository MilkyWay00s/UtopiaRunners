using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkill : MonoBehaviour
{
    public GameObject skillPrefab;
    public float cooldownTime = 5; // ��ų ��Ÿ��
    private bool isCooldown = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCooldown) // E ��ư�� ������ ��ų ����
        {
            UseSkill();
        }
    }

    void UseSkill()
    {
        Instantiate(skillPrefab, transform.position, transform.rotation);

        // ��Ÿ�� ����
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
        Debug.Log("Skill Ready");
    }
}
