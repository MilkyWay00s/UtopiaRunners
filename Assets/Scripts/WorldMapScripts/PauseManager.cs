using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    public GameObject OptionsPanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 옵션창이 열려 있으면 옵션창 닫기
            if (OptionsPanel.activeSelf)
            {
                onBackClicked();
                return;
            }

            // 일시정지 메뉴를 토글
            if (isPaused)
                onCloseClicked();
            else
                onPauseClicked();
        }
    }


    public void onPauseClicked()
    {
        PauseMenuPanel.SetActive(true);
        isPaused = true;
    }

    public void onCloseClicked()
    {
        PauseMenuPanel.SetActive(false);
        isPaused = false;
    }

    public void onOptionsClicked()
    {
        OptionsPanel.SetActive(true);
    }

    public void onBackClicked()
    {
        OptionsPanel.SetActive(false );
    }

    public void onMainMenuClicked()
    {
        SceneManager.LoadScene("Title");
    }
}
