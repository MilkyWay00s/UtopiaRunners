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
        if (WeaponSelectManager.Instance == null ||
    WeaponSelectManager.Instance.GetSelectedWeaponInfo() == null)
        {
            Debug.Log("무기를 선택해야 게임을 시작할 수 있습니다!");
            return;
        }

        else
        {
            SceneManager.LoadScene("7_InGameScene");
        }
    }

    public void onStageButtonClicked()
    {
        SceneManager.LoadScene("2_Eden");
    }
}
