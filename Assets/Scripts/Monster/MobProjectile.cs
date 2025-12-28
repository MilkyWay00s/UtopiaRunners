using UnityEngine;

public class MobProjectile : MonoBehaviour
{
    public float speed = 8f;

    Vector3 targetPos;
    bool hasTarget = false;

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
        hasTarget = true;
        Debug.Log("SetTarget »£√‚µ : " + target);
    }

    void Update()
    {
        if (!hasTarget) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
