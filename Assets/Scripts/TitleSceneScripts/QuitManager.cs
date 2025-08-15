﻿using UnityEngine;

public class QuitManager : MonoBehaviour
{
    public GameObject QuitPopup;  

    public void OnQuitButtonClicked()
    {
        QuitPopup.SetActive(true); 
    }

    public void OnYesClicked()
    {
        Application.Quit();         
        Debug.Log("게임 종료됨");
    }

    public void OnNoClicked()
    {
        QuitPopup.SetActive(false); 
    }
}
