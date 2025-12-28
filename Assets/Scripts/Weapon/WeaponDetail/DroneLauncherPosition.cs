using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLauncherPosition : MonoBehaviour
{
    private Transform playerPosition;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPosition = player.transform;
        }
    }

    void Update()
    {
        if (playerPosition != null)
        {
            Vector3 newPos = playerPosition.position;
            newPos.y += 2f;
            transform.position = newPos;
        }

        if (playerPosition != null && Input.GetKeyDown(KeyCode.Q))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
