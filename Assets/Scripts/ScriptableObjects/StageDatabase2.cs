using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/Stage Database 2", fileName = "StageDatabase2")]
public class StageDatabase2 : ScriptableObject
{
    public List<StageData2> stages = new();

    Dictionary<StageName, StageData2> map;

    void OnEnable()
    {
        BuildMap();
    }

    void BuildMap()
    {
        map = new Dictionary<StageName, StageData2>();
        foreach (var s in stages)
        {
            if (s != null)
                map[s.id] = s;
        }
    }

    public StageData2 Get(StageName id)
    {
        if (map == null || map.Count == 0)
            BuildMap();

        map.TryGetValue(id, out var data);
        return data;
    }
}
