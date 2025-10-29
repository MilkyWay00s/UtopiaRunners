using UnityEditor.SceneManagement;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public StageName targetStage;
    public void OnClickLoad()
    {
        StageManager.Instance.SelectAndPlay(targetStage);
    }
}