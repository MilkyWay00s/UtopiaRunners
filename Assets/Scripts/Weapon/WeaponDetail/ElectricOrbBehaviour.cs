using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOrbBehaviour : MonoBehaviour, IWeaponBehaviour
{
    public float damage = 15f;
    public float chainRange = 3f;
    public int chainCount = 2;

    bool chainReady = false;

    // 점프 시 PlayerController에서 호출
    public void EnableChainOnce()
    {
        chainReady = true;
    }

    public void OnHit(Vector3 hitPoint)
    {
        if (!chainReady) return;
        chainReady = false;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(hitPoint, chainRange);

        int count = 0;
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Boss")) continue;

            Enemy e = hit.GetComponent<Enemy>();
            if (e == null) continue;

            e.TakeDamage(damage);
            count++;
            if (count >= chainCount) break;
        }
    }
}