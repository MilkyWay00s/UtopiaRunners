using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager2 : MonoBehaviour
{
    public StageData2 stageData;
    private float timer = 0f;

    private int enemyWaveIndex = 0;
    private int obstacleWaveIndex = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= stageData.stageTimeLimit)
        {
            Debug.Log("Stage End");
            enabled = false;
            return;
        }

        // Enemy Wave 실행
        if (enemyWaveIndex < stageData.enemyWaves.Count)
        {
            var wave = stageData.enemyWaves[enemyWaveIndex];
            if (timer >= wave.startTime)
            {
                StartCoroutine(SpawnEnemyWave(wave));
                enemyWaveIndex++;
            }
        }

        // Obstacle Wave 실행
        if (obstacleWaveIndex < stageData.obstacleWaves.Count)
        {
            var wave = stageData.obstacleWaves[obstacleWaveIndex];
            if (timer >= wave.startTime)
            {
                StartCoroutine(SpawnObstacleWave(wave));
                obstacleWaveIndex++;
            }
        }
    }

    IEnumerator SpawnEnemyWave(EnemyWaveData wave)
    {
        foreach (var spawn in wave.spawnList)
        {
            yield return new WaitForSeconds(spawn.spawnTimeOffset);
            Instantiate(spawn.enemyPrefab, spawn.spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator SpawnObstacleWave(ObstacleWaveData wave)
    {
        foreach (var spawn in wave.spawnList)
        {
            yield return new WaitForSeconds(spawn.spawnTimeOffset);
            Instantiate(spawn.obstaclePrefab, spawn.spawnPosition, Quaternion.identity);
        }
    }
}
