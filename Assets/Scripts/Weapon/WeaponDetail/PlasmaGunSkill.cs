using UnityEngine;

public class PlasmaGunSkill : MonoBehaviour, IWeaponBehaviour
{
    public float directDamage = 20f;
    public float explosionDamage = 10f;
    public float explosionRadius = 2.5f;
    public float stunDuration = 1.0f;

    public void OnHit(Vector3 hitPoint)
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(hitPoint, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Boss")) continue;

            Enemy target = hit.GetComponent<Enemy>();
            if (target == null) continue;

            float dist =
                Vector3.Distance(hit.transform.position, hitPoint);

            if (dist < 0.1f)
            {
                target.TakeDamage(directDamage);
                target.ApplyStun(stunDuration);
            }
            else
            {
                target.TakeDamage(explosionDamage);
            }
        }
    }
}