using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkill : MonoBehaviour
{
    public WeaponData weaponData;
    private bool isCooldown = false;
    private float remainingCooldown;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCooldown)
        {
            UseSkill();
        }
    }

    void UseSkill()
    {
        Instantiate(weaponData.skillPrefab, transform.position, transform.rotation);
        remainingCooldown = weaponData.cooldownTime;
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        while (remainingCooldown > 0f)
        {
            remainingCooldown -= Time.deltaTime;
            yield return null;
        }

        isCooldown = false;
        Debug.Log("Skill Ready");
    }

    //이카루스 호출용
    public void ReduceCooldown(float amount)
    {
        if (!isCooldown) return;

        remainingCooldown = Mathf.Max(0f, remainingCooldown - amount);
    }
}
