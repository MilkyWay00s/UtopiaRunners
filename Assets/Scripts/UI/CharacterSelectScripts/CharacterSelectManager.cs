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

    private Image selectedCharacterDesplay;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject subPanel;

    private void Start()
    {
        //패널 초기화
        selectedCharacterDesplay = mainPanel.transform.Find("CharacterImage").GetComponent<Image>();

        //슬롯 초기화
        selectedCharacter = null;
        mainCharacter = null;
        subCharacter = null;
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
        selectedCharacter = ch;
        selectedCharacterDesplay.sprite = selectedCharacter.displayImage;
    }
    public void TagSelect()
    {
        SwapPanelSibling();
        if (mainPanel.transform.GetSiblingIndex() < subPanel.transform.GetSiblingIndex())
        {
            selectedCharacterDesplay = subPanel.transform.Find("CharacterImage").GetComponent<Image>();
        }
        else
        {
            selectedCharacterDesplay = mainPanel.transform.Find("CharacterImage").GetComponent<Image>();
        }
    }
    #endregion
}