using UnityEngine;

public class ObstacleMoving : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyX = -10f;

    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}
