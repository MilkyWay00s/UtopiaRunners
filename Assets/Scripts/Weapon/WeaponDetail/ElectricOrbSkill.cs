using UnityEngine;
using System.Collections;

public class ElectricOrbSkill : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public Vector2 direction = Vector2.right;

    [Header("Damage")]
    public float tickDamage = 5f;
    public float tickInterval = 0.3f;
    public float effectRadius = 2.5f;

    [Header("Shock")]
    public float stunDuration = 0.5f;

    [Header("Life Time")]
    public float lifeTime = 4f;

    void Start()
    {
        StartCoroutine(DamageTick());
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    IEnumerator DamageTick()
    {
        while (true)
        {
            Collider2D[] hits =
                Physics2D.OverlapCircleAll(transform.position, effectRadius);

            foreach (Collider2D hit in hits)
            {
                if (!hit.CompareTag("Boss")) continue;

                EnemyCondition enemy = hit.GetComponent<EnemyCondition>();
                if (enemy == null) continue;

                MobHealth mob = hit.GetComponent<MobHealth>();
                if (mob == null) continue;

                mob.TakeDamage(tickDamage);

                enemy.ApplyStun(stunDuration);
            }

            yield return new WaitForSeconds(tickInterval);
        }
    }

    // 디버그용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}