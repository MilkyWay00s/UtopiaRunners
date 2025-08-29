using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLauncherSkill : MonoBehaviour
{
    public WeaponAutoAttack weaponAutoAttack;
    public float duration = 5f;
    public float speedMultiplier = 2f;

    private void Start()
    {
        if (weaponAutoAttack != null)
        {
            StartCoroutine(ApplySkill());
        }
    }

    private IEnumerator ApplySkill()
    {
        float originalSpeed = weaponAutoAttack.weaponData.attackSpeed;

        weaponAutoAttack.weaponData.attackSpeed /= speedMultiplier;
        yield return new WaitForSeconds(duration);
        weaponAutoAttack.weaponData.attackSpeed = originalSpeed;

        Destroy(gameObject);
    }
}
