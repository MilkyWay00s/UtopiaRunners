using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 100f;
    private bool isStunned = false;

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0f)
        {
            Die();
        }
    }

    public void ApplyStun(float duration)
    {
        if (isStunned) return;
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;


        yield return new WaitForSeconds(duration);

        isStunned = false;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}