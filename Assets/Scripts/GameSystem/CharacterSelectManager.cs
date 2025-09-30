using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;

    [Header("Character Choice UI")]
    public GameObject characterButtonPanel;   // ��� �г�(�Ѱ� ����)
    public GameObject[] characters;     // A~D ��ư ������Ʈ

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

    // ������ ĳ���� ���á� ��ư
    public void OpenMainCharacterSelection(Button clickedButton)
    {
        isSelectingMain = true;
        curSelectedButton = clickedButton;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // ������ ĳ���� ���á� ��ư
    public void OpenSubCharacterSelection(Button clickedButton)
    {
        if (selectedMain == null)
        {
            Debug.Log("���� ĳ���͸� ���� �����ϼ���.");
            return;
        }
        isSelectingMain = false;
        curSelectedButton = clickedButton;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // ��Ͽ��� ĳ���� �ϳ��� ������ �� (�� ��ư OnClick�� index �Ҵ�)
    public void SelectCharacter(int index)
    {
        var spec = characterDatabase.allCharacters[index];

        if (isSelectingMain)
        {
            selectedMain = index;
            mainSlotImage.sprite = spec.displayImage; // ���� ���� �̹��� ����
            spec.selectedState = SelectedState.Main; // ���� �ʱ�ȭ �ʿ�
        }
        else
        {
            if (selectedMain == index)
            {
                Debug.Log("���ΰ� ���� ĳ���ʹ� ����� ������ �� �����ϴ�.");
                return;
            }
            selectedSub = index;
            subSlotImage.sprite = spec.displayImage; // ���� ���� �̹��� ����
            spec.selectedState = SelectedState.Sub;
        }

        characterButtonPanel.SetActive(false);
    }

    // ������ ���ۡ� ��ư
    public void StartGame()
    {
        if (selectedMain == null || selectedSub == null)
        {
            Debug.Log("����/���� ĳ���͸� ��� �����ϼ���.");
            return;
        }
        PlayerPrefs.SetInt("MainCharacter", selectedMain.Value);
        PlayerPrefs.SetInt("SubCharacter", selectedSub.Value);
        SceneManager.LoadScene("3_CharacterSelect");
    }

    // ���� ���� �� ���ΰ� ������ ��ư�� ��Ȱ��ȭ
    private void RefreshButtons()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(true);
            if (!isSelectingMain && selectedMain != null && i == selectedMain)
                characters[i].SetActive(false);
        }
    }
}