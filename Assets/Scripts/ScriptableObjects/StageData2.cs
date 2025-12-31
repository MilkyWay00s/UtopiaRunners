using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData2", menuName = "Stage/StageData2")]
public class StageData2 : ScriptableObject
{
    [Header("Stage Identity")]
    public StageName id;

    [Header("Stage Rule")]
    public float stageTimeLimit = 60f;
    public int stageRewardCoin = 100;

    [Header("Waves")]
    public List<EnemyWaveData> enemyWaves;
    public List<ObstacleWaveData> obstacleWaves;
}