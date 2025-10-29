using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectButton : MonoBehaviour
{
    public void LoadCharacterSelectScene()
    {
        SceneManager.LoadScene("3_CharacterSelect");
    }
}
