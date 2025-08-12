using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObstacle : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lifetime = 10f;
    private float timer;
    private int poolKey;
    public void Init(int key)
    {
        poolKey = key;
        timer = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ObjPoolManager.instance.ReturnObject(poolKey, gameObject);
        }
    }
}
