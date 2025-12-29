using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAutoAttack : MonoBehaviour
{
    public WeaponData weaponData;
    public float attackMultiplier = 1f;

    private void Start()
    {
        StartCoroutine(AutoAttack());
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            Instantiate(weaponData.autoAttackPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(weaponData.attackSpeed);
        }
    }
}