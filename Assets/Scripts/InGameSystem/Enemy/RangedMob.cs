using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMob : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float bulletSpeed = 5f;

    void Start()
    {
        InvokeRepeating(nameof(Shoot), fireRate, fireRate);
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Vector2 direction = (firePoint.position - bullet.transform.position).normalized;

        MobBulletMovement bm = bullet.GetComponent<MobBulletMovement>();
        if (bm != null)
        {
            bm.SetDirection(direction, bulletSpeed);
        }
    }
}



