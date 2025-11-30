using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleWave", menuName = "Stage/ObstacleWave")]
public class ObstacleWaveData : ScriptableObject
{
    public float startTime;
    public List<ObstacleSpawnInfo> spawnList;
}

[Serializable]
public class ObstacleSpawnInfo
{
    public GameObject obstaclePrefab;
    public float spawnTimeOffset;
    public Vector2 spawnPosition;
    public float moveSpeed = 1f;
}
