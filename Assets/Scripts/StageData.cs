using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage Data", fileName = "StageData_")]
public class StageData : ScriptableObject
{
    [Header("Identity")]
    public StageName id;
    public string displayName;
    [TextArea] public string description;

    [Header("Scene & Flow")]
    public string sceneName;         
    public float timeLimit = 120f;

    [Header("Player / Start")]
    public Vector2 playerStartPos = new Vector2(0, 0);
}