using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCount : MonoBehaviour
{
    public int missile = 0;
    PlayerController player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.Space))
        {
            if (player.IsGrounded())
            {
                missile += 1;
                Debug.Log("Missile = " + missile);
            }
        }

        if (player != null && Input.GetKeyDown(KeyCode.Q))
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            player = playerObj.GetComponent<PlayerController>();
        }
    }
}
