using System;
using System.Collections.Generic;

[Serializable]
public class StageClearData   
{
    public string world;
    public List<bool> stages;  
}

[Serializable]
public class SaveData
{
    public int coins;
    public float playTime;
    public string currentWorld;
    public string currentStage;

    public List<StageClearData> clearedStages = new List<StageClearData>();
}
