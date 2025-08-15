using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkill : MonoBehaviour
{
    public GameObject skillPrefab;
    public float cooldownTime = 5; // 스킬 쿨타임
    private bool isCooldown = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCooldown) // E 버튼을 누르면 스킬 시전
        {
            UseSkill();
        }
    }

    void UseSkill()
    {
        Instantiate(skillPrefab, transform.position, transform.rotation);

        // 쿨타임 시작
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
