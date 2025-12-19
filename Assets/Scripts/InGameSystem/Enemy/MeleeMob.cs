using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMob : MonoBehaviour
{
    public float speed = 5f;
    public float destroyTime = 5f;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}

