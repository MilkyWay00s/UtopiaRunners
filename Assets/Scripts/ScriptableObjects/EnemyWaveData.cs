using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Stage/EnemyWave")]
public class EnemyWaveData : ScriptableObject
{
    public float startTime;
    public List<EnemySpawnInfo> spawnList;
}

[Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public float spawnTimeOffset;
    public Vector2 spawnPosition;
    public float speedModifier = 1f;
}
