using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleSpawnPoint
{
    public Vector2 offset;
}

[CreateAssetMenu(fileName = "ObstaclePattern", menuName = "ObstaclePattern")]
public class ObstaclePattern : ScriptableObject, IBossPattern
{
    public string patternName;
    public GameObject obstaclePrefab;
    public ObstacleSpawnPoint[] spawnPoints;

    public float intervalAfterPattern => 2f;

    public IEnumerator ExecutePattern(Vector2 bossPos, MonoBehaviour executor)
    {
        foreach (var point in spawnPoints)
        {
            Vector2 spawnPos = bossPos + point.offset;
            GameObject.Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        }
        yield return null;
    }
}

