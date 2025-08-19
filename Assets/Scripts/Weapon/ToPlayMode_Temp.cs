using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToPlayMode_Temp : MonoBehaviour
{
    public void LoadPlayModeScene()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
