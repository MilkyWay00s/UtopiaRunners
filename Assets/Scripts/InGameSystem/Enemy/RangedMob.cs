using UnityEngine;

public class RangedMob : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    void Start()
    {
        InvokeRepeating(nameof(Shoot), fireRate, fireRate);
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null) return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
