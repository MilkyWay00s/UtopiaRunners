using UnityEngine;

public class TitleBGMManager : MonoBehaviour
{
    void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM("MainTheme");
        }
        else
        {
            Debug.LogWarning("SoundManager가 씬에 존재하지 않습니다!");
        }
    }
}