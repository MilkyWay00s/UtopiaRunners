using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int coins;
    public float playTime;
    public string currentWorld;
    public string currentStage;
    public Dictionary<string, List<bool>> clearedStages = new Dictionary<string, List<bool>>();
}