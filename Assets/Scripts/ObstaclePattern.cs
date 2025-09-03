using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleSpawnPoint
{
    public Vector2 offset;
}

[CreateAssetMenu(fileName = "ObstaclePattern", menuName = "ObstaclePattern")]
public class ObstaclePattern : ScriptableObject
{
    public string patternName;
    public GameObject obstaclePrefab;
    public ObstacleSpawnPoint[] spawnPoints;

    public void SpawnPattern(Vector2 bossPos)
    {
        foreach (var point in spawnPoints)
        {
            Vector2 spawnPos = bossPos + point.offset;
            GameObject.Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        }
    }
}

