using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotManager : MonoBehaviour
{
    public GameObject SaveSlotPanel;

    public void OnStartButtonClicked()
    {
        SaveSlotPanel.SetActive(true);
    }

    public void onBackButtonClicked()
    {
        SaveSlotPanel.SetActive(false);
    }

    public void onSelectButtonClicked()
    {
        SceneManager.LoadScene("WorldMap");
    }
}
