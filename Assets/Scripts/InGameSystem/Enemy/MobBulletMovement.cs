using UnityEngine;

public class MobBulletMovement : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;

    void Start()
    {
        GameObject targetObj = GameObject.FindGameObjectWithTag("Target");
        if (targetObj != null)
        {
            target = targetObj.transform;
        }

        Destroy(gameObject, 3f);
    }


    void Update()
    {
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }
}
