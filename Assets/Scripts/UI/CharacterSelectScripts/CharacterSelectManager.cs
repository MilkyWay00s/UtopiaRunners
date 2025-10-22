using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : SingletonObject<CharacterSelectManager>
{
    [SerializeField] private CharacterDatabase characterDatabase;

    [SerializeField] private CharacterSpec selectedCharacter;

    [SerializeField] private CharacterSpec mainCharacter;
    [SerializeField] private CharacterSpec subCharacter;
    //select scene에서 캐릭터 선택 시 저장 될 슬롯

    [SerializeField] private Image selectedCharacterDesplay;

    private void Start()
    {
        selectedCharacter = null;
        mainCharacter = null;
        subCharacter = null;
    }
    public void OnUpgradeButtonClicked()
    {
        SceneManager.LoadScene("5_Upgrade"); 
    }
    public void CharacterSelect(CharacterSpec ch)
    {
        selectedCharacter = ch;
        selectedCharacterDesplay.sprite = selectedCharacter.displayImage;
    }
}
