using System.Collections;
using System.Collections.Generic;
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
        string currentWorld = GameManager.Instance.currentWorld;
        SceneManager.LoadScene(currentWorld);
    }
}
