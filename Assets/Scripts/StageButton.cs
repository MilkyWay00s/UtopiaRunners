using UnityEditor.SceneManagement;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    public StageName targetStage;   // �ν����Ϳ��� ����
    public void OnClickLoad()
    {
        StageManager.Instance.SelectAndPlay(targetStage);
    }
}