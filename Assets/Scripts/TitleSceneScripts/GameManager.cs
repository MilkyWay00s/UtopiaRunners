using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private string saveFolder;

    public int coins = 0;
    public int currentSlot = 1;

    public float playTime = 0f;  
    private float sessionStartTime;

    public string currentWorld = "World1";
    public string currentStage = "Stage1";
    public List<StageClearData> clearedStages = new List<StageClearData>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            saveFolder = Application.persistentDataPath;
            sessionStartTime = Time.time;

            int recentSlot = GetMostRecentSlot();
            LoadGame(recentSlot);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void SaveGame(int slot)
    {
        UpdatePlayTime();

        SaveData data = new SaveData
        {
            coins = coins,
            playTime = playTime,
            currentWorld = currentWorld,
            currentStage = currentStage,
            clearedStages = clearedStages
        .Select(kvp => new StageClearData
        {
            world = kvp.Key,
            stages = kvp.Value
        })
        .ToList()
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
            clearedStages = data.clearedStages
            .ToDictionary(entry => entry.world, entry => entry.stages);
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



    public void CompleteStage(string world, int stageIndex)
    {
        // 해당 world 데이터를 찾음
        StageClearData worldData = clearedStages.Find(w => w.world == world);

        // 없으면 새로 추가
        if (worldData == null)
        {
            worldData = new StageClearData
            {
                world = world,
                stages = new List<bool>()
            };
            clearedStages.Add(worldData);
        }

        // stageIndex까지 리스트 크기 늘리기
        while (worldData.stages.Count <= stageIndex)
        {
            worldData.stages.Add(false);
        }

        // 해당 스테이지를 클리어 처리
        worldData.stages[stageIndex] = true;

        // 현재 진행 상황 갱신
        currentWorld = world;
        currentStage = $"Stage{stageIndex + 1}";

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
    }

    private void OnApplicationQuit()
    {
        SaveGame(currentSlot);
    }
}