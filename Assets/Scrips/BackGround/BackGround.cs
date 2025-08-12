using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewWide;
    private void Awake()
    {
        viewWide = Camera.main.orthographicSize * Camera.main.aspect;
    }
    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (sprites[startIndex].position.x < viewWide*(-1))
        {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[startIndex].transform.localPosition = frontSpritePos + Vector3.right* viewWide;

            int startIndexSave = startIndex;
            startIndex = endIndex-1;
            endIndex = (startIndexSave-1 == -1)? sprites.Length-1:startIndexSave-1;
        }
    }
}
