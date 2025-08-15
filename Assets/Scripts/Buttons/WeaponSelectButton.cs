using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSelectButton : MonoBehaviour
{
    public void LoadWeaponSelectScene()
    {
        SceneManager.LoadScene("WeaponSelectMode");
    }
}
