using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterPortrait : MonoBehaviour, ICharacterPortrait
{
    [Header("캐릭터")]
    public CharacterSpec character;
    [Header("캐릭터 초상화")]
    public Image characterImage;

    void Start()
    {
        characterImage.sprite = character.displayImage;
    }
    public void ClickedPortrait()
    {
        CharacterSelectManager.Instance.CharacterSelect(character);
    }
}