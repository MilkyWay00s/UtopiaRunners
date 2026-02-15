using UnityEngine;

public class Feather : MonoBehaviour
{
    private bool isLaunched = false;
    private Vector3 targetDirection;
    private Rigidbody2D rb;
    public float speed = 20f;
    public float life = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector3 targetPos)
    {
        Vector3 dir = (targetPos - transform.position).normalized;

        rb.velocity = dir * speed;

        Destroy(gameObject, life);
    }
}