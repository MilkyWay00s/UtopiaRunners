using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackProjectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Transform targetBoss; 

    private void Start()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        targetBoss = boss.transform;
    }

    private void Update()
    {
        Vector3 direction = (targetBoss.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ������ �浹�Ǹ� ����
        if (other.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
