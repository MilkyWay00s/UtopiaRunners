using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;

    [Header("Character Choice UI")]
    public GameObject characterButtonPanel;   // 목록 패널(켜고 끄기)
    public GameObject[] characterButtons;     // A~D 버튼 오브젝트

    [Header("Slot UI")]
    [SerializeField] private Image mainSlotImage;
    [SerializeField] private Image subSlotImage;

    private Button curSelectedButton;

    private int? selectedMain = null;
    private int? selectedSub = null;
    private bool isSelectingMain = true;

    [SerializeField] private Sprite emptySlotSprite;
    void Start()
    {
        characterButtonPanel.SetActive(false);
        mainSlotImage.sprite = emptySlotSprite;
        subSlotImage.sprite = emptySlotSprite;
    }

    // “메인 캐릭터 선택” 버튼
    public void OpenMainCharacterSelection(Button clickedButton)
    {
        isSelectingMain = true;
        curSelectedButton = clickedButton;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // “서브 캐릭터 선택” 버튼
    public void OpenSubCharacterSelection(Button clickedButton)
    {
        if (selectedMain == null)
        {
            Debug.Log("메인 캐릭터를 먼저 선택하세요.");
            return;
        }
        isSelectingMain = false;
        curSelectedButton = clickedButton;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // 목록에서 캐릭터 하나를 눌렀을 때 (각 버튼 OnClick에 index 할당)
    public void SelectCharacter(int index)
    {
        var spec = characterDatabase.allCharacters[index];

        if (isSelectingMain)
        {
            selectedMain = index;
            mainSlotImage.sprite = spec.displayImage; // 메인 슬롯 이미지 갱신
            spec.selectedState = SelectedState.Main; // 이후 초기화 필요
        }
        else
        {
            if (selectedMain == index)
            {
                Debug.Log("메인과 같은 캐릭터는 서브로 선택할 수 없습니다.");
                return;
            }
            selectedSub = index;
            subSlotImage.sprite = spec.displayImage; // 서브 슬롯 이미지 갱신
            spec.selectedState = SelectedState.Sub;
        }

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
        SceneManager.LoadScene("3_CharacterSelect");
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