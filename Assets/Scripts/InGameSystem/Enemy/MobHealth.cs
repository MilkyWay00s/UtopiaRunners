using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    public float maxHP = 3;
    public float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            AutoAttackProjectile projectile =
                other.GetComponent<AutoAttackProjectile>();

            if (projectile != null)
            {
                TakeDamage(projectile.finalDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
