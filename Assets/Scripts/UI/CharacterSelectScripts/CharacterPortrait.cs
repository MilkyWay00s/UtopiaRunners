using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortrait : MonoBehaviour, ICharacterPortrait {
    [Header("캐릭터")]
    public CharacterSpec characterSpec;
    [Header("캐릭터 이미지")]
    public Image characterImage;

    void Start() {
        characterImage.sprite = characterSpec.displayImage;
    }
    public CharacterSpec ClickPortrait() {
        return this.characterSpec;
    }
}
