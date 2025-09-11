using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkill : MonoBehaviour
{
    public WeaponData weaponData;
    private bool isCooldown = false;

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
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(weaponData.cooldownTime);
        isCooldown = false;
        Debug.Log("Skill Ready");
    }
}
