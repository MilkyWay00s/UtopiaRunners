using UnityEngine;
using System.Collections;

public class MobLaser : MonoBehaviour
{
    float currentLength = 0f;
    float growSpeed;
    float maxLength;
    float duration;

    SpriteRenderer sr;
    BoxCollider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    public void Init(float growSpeed, float maxLength, float duration)
    {
        this.growSpeed = growSpeed;
        this.maxLength = maxLength;
        this.duration = duration;

        currentLength = 0f;
        SetLength(0f);

        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (currentLength >= maxLength) return;

        currentLength += growSpeed * Time.deltaTime;
        SetLength(currentLength);
    }

    void SetLength(float length)
    {
        sr.size = new Vector2(length, sr.size.y);
        col.size = new Vector2(length, col.size.y);

        col.offset = Vector2.zero;
    }

}
