using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    public GameObject clearPanel;

    public void onClickClear()
    {
        Time.timeScale = 0f;
        clearPanel.SetActive(true);
    }

    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void OnClickNext()
    {
        Time.timeScale = 1f;

        string world = GameManager.Instance.currentWorld;
        string stageStr = GameManager.Instance.currentStage; 
        int stageIndex = int.Parse(stageStr.Replace("Stage", "")) - 1;

        GameManager.Instance.CompleteStage(world, stageIndex);

        if (GameManager.Instance.clearedStages.ContainsKey(world))
        {
            List<bool> stages = GameManager.Instance.clearedStages[world];
            string status = string.Join(", ", stages.Select(b => b ? "Cleared" : "NotCleared"));
            Debug.Log($"[{world}] 스테이지 클리어 상태: {status}");
        }

        string currentWorld = GameManager.Instance.currentWorld;
        SceneManager.LoadScene(currentWorld);
    }
}
