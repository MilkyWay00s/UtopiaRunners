using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public StageName targetStage;

 
    public void OnClickSelect()
    {
        StageManager.Instance.SelectAndLoad(targetStage);
    }
}