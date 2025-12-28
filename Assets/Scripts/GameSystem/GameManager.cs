using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class WorldClearEntry
{
    public string worldName;
    public List<bool> cleared; // 스테이지 클리어 여부
}

[Serializable]
public class SaveData
{
    public int coins;
    public string currentWorld;
    public string currentStage;
    public float playTime;

    public List<WorldClearEntry> clearedWorlds = new List<WorldClearEntry>();
}
public class GameManager : SingletonObject<GameManager>
{
    private string saveFolder;

    public int coins = 0;
    public int currentSlot = 1;

    public float playTime = 0f;  
    private float sessionStartTime;

    public string currentWorld = "World1";
    public string currentStage = "Stage1";
    public Dictionary<string, List<bool>> clearedStages = new Dictionary<string, List<bool>>();

    [Header("Databases")]
    public StageDatabase stageDatabase;

    [Header("Runtime Selection")]
    [SerializeField] private StageName selectedStageId;
    public StageName SelectedStageId => selectedStageId;

    public event Action<StageData> OnStageSelected;

    protected override void Awake()
    {
        base.Awake();
        saveFolder = Application.persistentDataPath;
        sessionStartTime = Time.time;

        int recentSlot = GetMostRecentSlot();
        LoadGame(recentSlot);

        selectedStageId = ParseStageNameFromString(currentStage);
    }

    private void UpdatePlayTime()
    {
        playTime += Time.time - sessionStartTime;
        sessionStartTime = Time.time;
    }

    public void SetCurrentSlot(int slot)
    {
        currentSlot = slot;
    }
    private List<WorldClearEntry> ConvertDictToList(Dictionary<string, List<bool>> dict)
    {
        var list = new List<WorldClearEntry>();
        foreach (var kv in dict)
        {
            list.Add(new WorldClearEntry
            {
                worldName = kv.Key,
                cleared = kv.Value
            });
        }
        return list;
    }

    private Dictionary<string, List<bool>> ConvertListToDict(List<WorldClearEntry> list)
    {
        var dict = new Dictionary<string, List<bool>>();
        if (list == null) return dict;

        foreach (var e in list)
        {
            if (string.IsNullOrEmpty(e.worldName)) continue;
            dict[e.worldName] = e.cleared ?? new List<bool>();
        }
        return dict;
    }
    public void SaveGame(int slot)
    {
        UpdatePlayTime();

        SaveData data = new SaveData
        {
            coins = coins,
            currentWorld = currentWorld,
            currentStage = currentStage,
            playTime = playTime,
            clearedWorlds = ConvertDictToList(clearedStages)
        };

        string path = Path.Combine(saveFolder, $"save{slot}.json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"게임 저장 완료 (슬롯 {slot})");
    }
    public void LoadGame(int slot)
    {
        currentSlot = slot;
        string path = Path.Combine(saveFolder, $"save{slot}.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            coins = data.coins;
            currentWorld = data.currentWorld;
            currentStage = data.currentStage;
            playTime = data.playTime;

            clearedStages = ConvertListToDict(data.clearedWorlds);

            selectedStageId = ParseStageNameFromString(currentStage);
        }
        else
        {
            NewGame();
        }
    }


    public int GetMostRecentSlot()
    {
        string folder = Application.persistentDataPath;
        string[] files = Directory.GetFiles(folder, "save*.json");

        if (files.Length == 0)
        {
            return 1; 
        }

        FileInfo recentFile = files
            .Select(f => new FileInfo(f))
            .OrderByDescending(f => f.LastWriteTime)
            .First();

        string fileName = Path.GetFileNameWithoutExtension(recentFile.Name);
        int slotNumber = int.Parse(fileName.Replace("save", ""));

        return slotNumber;
    }

    public void SetCurrentWorld(string worldName)
    {
        currentWorld = worldName;

        SaveGame(currentSlot);
    }

    public void SetCurrentStage(int stageIndex)
    {
        currentStage = $"Stage{stageIndex + 1}";

        SaveGame(currentSlot); 
    }
    //선택된 스테이지 전달받는 매서드
    public void SelectStage(StageName stageId, bool save = true)
    {
        selectedStageId = stageId;

        // 저장용 string도 같이 맞춰줌
        currentStage = StageNameToSaveString(stageId);

        if (save) SaveGame(currentSlot);

        // 런타임 StageData 이벤트로 뿌려줌
        var data = GetSelectedStageData();
        if (data != null) OnStageSelected?.Invoke(data);
    }
    public StageData GetSelectedStageData()
    {
        if (stageDatabase == null)
        {
            Debug.LogError("GameManager: stageDatabase가 할당되지 않았습니다.");
            return null;
        }
        return stageDatabase.GetStageName(selectedStageId);
    }
    public void CompleteStage(string world, int stageIndex)
    {
        if (!clearedStages.ContainsKey(world))
            clearedStages[world] = new List<bool>();

        while (clearedStages[world].Count <= stageIndex)
            clearedStages[world].Add(false);

        clearedStages[world][stageIndex] = true;

        currentWorld = world;
        currentStage = $"Stage{stageIndex + 1}";

        selectedStageId = ParseStageNameFromString(currentStage);

        SaveGame(currentSlot); 
    }


    public void LoadMostRecent()
    {
        int slot = GetMostRecentSlot();
        LoadGame(slot); 
    }


    public void NewGame()
    {
        coins = 10000;
        currentWorld = "World1";
        currentStage = "Stage1";
        selectedStageId = ParseStageNameFromString(currentStage);
    }

    private void OnApplicationQuit()
    {
        SaveGame(currentSlot);
    }
    private StageName ParseStageNameFromString(string stageStr)
    {
        if (string.IsNullOrEmpty(stageStr)) return default;
        if (Enum.TryParse(stageStr, out StageName parsed))
            return parsed;

        return default;
    }

    private string StageNameToSaveString(StageName id)
    {
        return id.ToString();
    }
    //클리어 판정을 위한 로직-----------------------------------------------------------
    public bool IsStageCleared(string worldName, int stageNumber)
    {
        if (!clearedStages.ContainsKey(worldName)) return false;
        var list = clearedStages[worldName];

        int idx = stageNumber - 1;
        if (idx < 0 || idx >= list.Count) return false;

        return list[idx];
    }
    public bool IsStageUnlocked(string worldName, int stageIndex)
    {
        if (stageIndex <= 1) return true; // Stage1은 항상 오픈
        return IsStageCleared(worldName, stageIndex - 1);
    }
    // 월드 마지막 스테이지 클리어 시 해금
    public bool IsWorldCleared(string worldName, int lastStageIndex)
    {
        return IsStageCleared(worldName, lastStageIndex);
    }

    public bool IsWorldUnlocked(int worldIndex, int stagesPerWorld)
    {
        if (worldIndex <= 1) return true; // 첫 월드는 항상 오픈

        string prevWorld = $"World{worldIndex}"; // worldIndex=1이면 이전 월드는 World1

        return IsWorldCleared(prevWorld, stagesPerWorld - 1);
    }
    //-----------------------------------------------------------
    public int GetLastEnteredStageNumber()
    {
        if (string.IsNullOrEmpty(currentStage)) return 1;

        string numStr = currentStage.Replace("Stage", "");
        if (int.TryParse(numStr, out int stageNumber))
            return Mathf.Max(stageNumber, 1);

        return 1;
    }
}