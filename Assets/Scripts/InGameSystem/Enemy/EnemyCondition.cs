using System.Collections;
using UnityEngine;

public class EnemyCondition : MonoBehaviour
{
    private bool isStunned = false;


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