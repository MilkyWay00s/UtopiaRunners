using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSceneManager : MonoBehaviour
{
    public GameObject weaponSelectUI;
    public GameObject characterSelectUI;

    public void OnWeaponButtonClicked()
    {
        weaponSelectUI.SetActive(true);
        characterSelectUI.SetActive(false);
    }

    public void OnSelectButtonClicked()
    {
        weaponSelectUI.SetActive(false);
        characterSelectUI.SetActive(true);
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
