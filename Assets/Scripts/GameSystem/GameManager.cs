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
    public Dictionary<string, List<bool>> clearedStages = new Dictionary<string, List<bool>>();


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
            currentWorld = currentWorld,
            currentStage = currentStage,
            clearedStages = clearedStages,
            playTime = playTime
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
            clearedStages = data.clearedStages;
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
        if (!clearedStages.ContainsKey(world))
            clearedStages[world] = new List<bool>();

        while (clearedStages[world].Count <= stageIndex)
            clearedStages[world].Add(false);

        clearedStages[world][stageIndex] = true;

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