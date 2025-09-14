using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage Database", fileName = "StageDatabase")]
public class StageDatabase : ScriptableObject
{
    public List<StageData> stages = new List<StageData>();

    private Dictionary<StageName, StageData> _map;
    public void BuildMap()
    {
        _map = new Dictionary<StageName, StageData>();
        foreach (var s in stages)
        {
            if (s == null) continue;
            _map[s.id] = s;
        }
    }

    public StageData GetStageId(StageName id)
    {
        if (_map == null) BuildMap();
        return _map != null && _map.TryGetValue(id, out var data) ? data : null;
    }

    public StageData GetByIndex(int index)
    {
        if (stages == null || stages.Count == 0) return null;
        index = Mathf.Clamp(index, 0, stages.Count - 1);
        return stages[index];
    }
}