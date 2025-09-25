using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CharacterPortrait : MonoBehaviour, ICharacterPortrait
{
    [Header("ĳ����")]
    public CharacterSpec character;
    [Header("ĳ���� �ʻ�ȭ")]
    public SpriteRenderer characterImage;

    void Start()
    {
        characterImage.sprite = character.displayImage;
    }
    public CharacterSpec ClickPortrait()
    {
        return this.character;
    }
}