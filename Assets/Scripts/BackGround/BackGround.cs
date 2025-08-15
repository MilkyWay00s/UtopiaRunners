using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float spriteWidth;

    private void Awake()
    {
        spriteWidth = sprites[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (sprites[startIndex].position.x < -spriteWidth)
        {
            Vector3 frontPos = sprites[endIndex].position;
            sprites[startIndex].position = frontPos + Vector3.right * spriteWidth;
            endIndex = startIndex;
            startIndex = (startIndex + 1) % sprites.Length;
        }
    }
}
