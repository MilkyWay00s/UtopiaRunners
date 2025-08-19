using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Character Choice UI")]
    public GameObject characterButtonPanel;   // 목록 패널(켜고 끄기)
    public GameObject[] characterButtons;     // A~D 버튼 오브젝트
    

    private int? selectedMain = null;
    private int? selectedSub = null;
    private bool isSelectingMain = true;

    void Start()
    {
        characterButtonPanel.SetActive(false);

    }

    // “메인 캐릭터 선택” 버튼
    public void OpenMainCharacterSelection()
    {
        isSelectingMain = true;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // “서브 캐릭터 선택” 버튼
    public void OpenSubCharacterSelection()
    {
        if (selectedMain == null)
        {
            Debug.Log("메인 캐릭터를 먼저 선택하세요.");
            return;
        }
        isSelectingMain = false;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // 목록에서 캐릭터 하나를 눌렀을 때 (각 버튼 OnClick에 index 할당)
    public void SelectCharacter(int index)
    {
        if (isSelectingMain)
        {
            selectedMain = index;
        }
        else
        {
            if (selectedMain == index)
            {
                Debug.Log("메인과 같은 캐릭터는 서브로 선택할 수 없습니다.");
                return;
            }
            selectedSub = index;
        }

        // 선택 후 패널 닫기 (원하면 유지해도 됨)
        characterButtonPanel.SetActive(false);
    }

    // “게임 시작” 버튼
    public void StartGame()
    {
        if (selectedMain == null || selectedSub == null)
        {
            Debug.Log("메인/서브 캐릭터를 모두 선택하세요.");
            return;
        }
        PlayerPrefs.SetInt("MainCharacter", selectedMain.Value);
        PlayerPrefs.SetInt("SubCharacter", selectedSub.Value);
        SceneManager.LoadScene("CharacterSelect");
    }

    // 서브 선택 시 메인과 동일한 버튼만 비활성화
    private void RefreshButtons()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            characterButtons[i].SetActive(true);
            if (!isSelectingMain && selectedMain != null && i == selectedMain)
                characterButtons[i].SetActive(false);
        }
    }
}