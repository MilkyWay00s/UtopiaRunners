using UnityEngine;
using System.Collections; 

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject DarkOverlay;
    public GameObject MenuPanel;
    public GameObject SettingsPanel;

    private Animator settingsAnimator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingsPanel.activeSelf)
            {
                BackToMainMenu();
            }
            else
            {
                ToggleOptions();
            }
        }
    }

    public void ToggleOptions()
    {
        bool isActive = !MenuPanel.activeSelf;

        DarkOverlay.SetActive(isActive);
        MenuPanel.SetActive(isActive);

        Time.timeScale = isActive ? 0f : 1f;

        if (!isActive)
        {
            SettingsPanel.SetActive(false);
        }
    }

    public void OnSettingsButtonClicked()
    {
        MenuPanel.SetActive(false); 
        SettingsPanel.SetActive(true); 

        settingsAnimator = SettingsPanel.GetComponent<Animator>();
        if (settingsAnimator != null)
        {
            settingsAnimator.SetTrigger("Open");
        }
    }

    public void BackToMainMenu()
    {
        if (settingsAnimator != null)
        {
            settingsAnimator.SetTrigger("Close");
            StartCoroutine(DisableSettingsPanel(0.1f)); 
        }
        else
        {
            SettingsPanel.SetActive(false);
            MenuPanel.SetActive(true);
        }
    }

    IEnumerator DisableSettingsPanel(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        SettingsPanel.SetActive(false);
        if (DarkOverlay.activeSelf)
        {
            MenuPanel.SetActive(true);
        }
    }

    public void OnPlayButtonClicked() => ToggleOptions();

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}