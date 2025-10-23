using UnityEditor.SceneManagement;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public StageName targetStage;   // 인스펙터에서 선택
    public void OnClickLoad()
    {
        StageManager.Instance.SelectAndPlay(targetStage);
    }
}