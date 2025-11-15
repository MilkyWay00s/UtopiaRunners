using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBulletMovement : MonoBehaviour
{
    private Vector2 moveDirection;
    private float speed;

    public void SetDirection(Vector2 dir, float spd)
    {
        moveDirection = dir.normalized;
        speed = spd;
    }

    void Update()
    {
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }
}
