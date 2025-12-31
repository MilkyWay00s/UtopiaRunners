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

            EnemyCondition enemy = hit.GetComponent<EnemyCondition>();
            if (enemy == null) continue;

            MobHealth mob = hit.GetComponent<MobHealth>();
            if (mob == null) continue;

            float dist =
                Vector3.Distance(hit.transform.position, hitPoint);

            if (dist < 0.1f)
            {
                mob.TakeDamage(directDamage);
                enemy.ApplyStun(stunDuration);
            }
            else
            {
                mob.TakeDamage(explosionDamage);
            }
        }
    }
}