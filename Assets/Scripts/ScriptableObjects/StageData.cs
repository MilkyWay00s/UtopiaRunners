using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage Data", fileName = "StageData_")]
public class StageData : ScriptableObject
{
    [Header("Identity")]
    public StageName id;
    public string displayName;
    [TextArea] public string description;

    [Header("Environment")]
    public Sprite background;
    public Color ambientLight = Color.white;

    /*[Header("Audio")]
    public AudioClip bgm;
    [Range(0f, 1f)] public float bgmVolume = 0.6f;
    */
    [Header("Spawn/Difficulty")]
    public int difficulty = 1;
    public GameObject[] enemyPrefabs;
    public AnimationCurve spawnRateOverTime;
    public float baseSpawnInterval = 2f;

    [Header("Player Start")]
    public Vector2 playerStartPos = Vector2.zero;
}