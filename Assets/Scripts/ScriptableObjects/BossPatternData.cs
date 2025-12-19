using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnInfo
{
    public GameObject prefab;
    public Vector3 offset;
    public float delay;

    [Header("Rotation Settings")]
    public float rotationZ = 0f;
}

[CreateAssetMenu(fileName = "BossPatternData", menuName = "Boss/BossPatternData")]
public class BossPatternData : ScriptableObject
{
    public string patternName;

    public List<SpawnInfo> spawnInfos = new List<SpawnInfo>();
    public float cooldown = 1f;
}
