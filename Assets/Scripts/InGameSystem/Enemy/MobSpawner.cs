using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform meleeSpawnPoint;
    public Transform[] rangedSpawnPoints;

    [Header("Mob Prefabs")]
    public GameObject[] meleeMobs;
    public GameObject[] rangedMobs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public bool isSpawning = true;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnMob();
        }
    }

    void SpawnMob()
    {
        int totalPoints = 1 + rangedSpawnPoints.Length;

        int chosenIndex = Random.Range(0, totalPoints);

        if (chosenIndex == 0)
        {
            SpawnMeleeMob();
        }
        else
        {
            SpawnRangedMob(chosenIndex - 1);
        }
    }

    void SpawnMeleeMob()
    {
        if (meleeMobs.Length == 0 || meleeSpawnPoint == null) return;

        int mobIndex = Random.Range(0, meleeMobs.Length);
        Instantiate(meleeMobs[mobIndex], meleeSpawnPoint.position, Quaternion.identity);
    }

    void SpawnRangedMob(int index)
    {
        if (rangedMobs.Length == 0 || rangedSpawnPoints.Length == 0) return;

        Transform point = rangedSpawnPoints[index];
        int mobIndex = Random.Range(0, rangedMobs.Length);

        Instantiate(rangedMobs[mobIndex], point.position, Quaternion.identity);
    }
}
