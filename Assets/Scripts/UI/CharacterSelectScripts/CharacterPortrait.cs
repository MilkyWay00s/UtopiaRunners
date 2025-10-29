using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterPortrait : MonoBehaviour, ICharacterPortrait
{
    [Header("ĳ����")]
    public CharacterSpec character;
    [Header("ĳ���� �ʻ�ȭ")]
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