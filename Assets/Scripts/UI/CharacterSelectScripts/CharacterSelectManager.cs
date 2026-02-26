using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;

    [SerializeField] private CharacterSpec selectedCharacter;

    [SerializeField] private CharacterSpec mainCharacter;
    [SerializeField] private CharacterSpec subCharacter;
    //select scene에서 캐릭터 선택 시 저장 될 슬롯

    private Image selectedCharacterDesplay;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject subPanel;

    private bool selectingMain = true;

    private void Start()
    {
        int mainIndex = PlayerPrefs.GetInt("MainCharacter", -1);
        int subIndex = PlayerPrefs.GetInt("SubCharacter", -1);

        if (mainIndex >= 0)
            mainCharacter = characterDatabase.allCharacters[mainIndex];

        if (subIndex >= 0)
            subCharacter = characterDatabase.allCharacters[subIndex];

        selectingMain = true;
    }



    public void SwapPanelSibling()
    {
        int m_idx = mainPanel.transform.GetSiblingIndex();
        int s_idx = subPanel.transform.GetSiblingIndex();
        mainPanel.transform.SetSiblingIndex(s_idx);
        subPanel.transform.SetSiblingIndex(m_idx);
    }

    #region 버튼 이벤트
    public void OnUpgradeButtonClicked()
    {
        SceneManager.LoadScene("5_Upgrade");
    }
    public void CharacterSelect(CharacterSpec ch)
    {
        if (selectingMain)
        {
            mainCharacter = ch;
            PlayerPrefs.SetInt("MainCharacter", ch.characterIndex);

            mainPanel.transform.Find("CharacterImage")
                .GetComponent<Image>().sprite = ch.displayImage;
        }
        else
        {
            subCharacter = ch;
            PlayerPrefs.SetInt("SubCharacter", ch.characterIndex);

            subPanel.transform.Find("CharacterImage")
                .GetComponent<Image>().sprite = ch.displayImage;
        }

        PlayerPrefs.Save();
    }

    public void TagSelect()
    {
        selectingMain = !selectingMain;
        SwapPanelSibling();
    }



    #endregion
}