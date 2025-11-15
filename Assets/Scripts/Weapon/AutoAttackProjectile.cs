using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackProjectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public string targetTag = "Boss";

    private Transform target;

    private void Start()
    {
        AcquireTarget();
    }

    private void Update()
    {
        if (target == null)
        {
            AcquireTarget();

            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void AcquireTarget()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetTag);
        if (possibleTargets.Length == 0) return;

        float minDist = Mathf.Infinity;
        GameObject nearest = null;
        foreach (var obj in possibleTargets)
        {
            float dist = Vector2.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = obj;
            }
        }

        if (nearest != null)
            target = nearest.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
