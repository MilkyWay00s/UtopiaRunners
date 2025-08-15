using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSelect : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;

    private int currentSlot = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentSlot != 2)
            {
                transform.position = slot2.transform.position;
                currentSlot = 2;
            }
        }

        // 왼쪽 방향키 입력
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentSlot != 1)
            {
                transform.position = slot1.transform.position;
                currentSlot = 1;
            }
        }
    }
}
