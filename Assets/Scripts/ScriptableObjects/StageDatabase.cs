using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage Database", fileName = "StageDatabase")]
public class StageDatabase : ScriptableObject
{
    public List<StageData> stages = new();
    Dictionary<StageName, StageData> map;

    void OnEnable() => BuildMap();

    public void BuildMap()
    {
        map = new();
        foreach (var s in stages) if (s) map[s.id] = s;
    }

    public StageData GetStageName(StageName id)
    {
        if (map == null || map.Count == 0) BuildMap();
        return map != null && map.TryGetValue(id, out var data) ? data : null;
    }
}