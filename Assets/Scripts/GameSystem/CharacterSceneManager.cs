using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSceneManager : MonoBehaviour
{
    public void onWeaponButtonClicked()
    {
        SceneManager.LoadScene("6_WeaponSelectMode");
    }

    public void onGoButtonClicked()
    {
        SceneManager.LoadScene("7_InGameScene");
    }

    public void onStageButtonClicked()
    {
        SceneManager.LoadScene("2_Eden");
    }
}
