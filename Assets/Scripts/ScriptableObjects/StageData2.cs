using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData2", menuName = "Stage/StageData2")]
public class StageData2 : ScriptableObject
{
    public float stageTimeLimit = 60f;
    public List<EnemyWaveData> enemyWaves;
    public List<ObstacleWaveData> obstacleWaves;
}