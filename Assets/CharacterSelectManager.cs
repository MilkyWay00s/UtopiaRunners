using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Character Choice UI")]
    public GameObject characterButtonPanel;   // ��� �г�(�Ѱ� ����)
    public GameObject[] characterButtons;     // A~D ��ư ������Ʈ
    

    private int? selectedMain = null;
    private int? selectedSub = null;
    private bool isSelectingMain = true;

    void Start()
    {
        characterButtonPanel.SetActive(false);

    }

    // ������ ĳ���� ���á� ��ư
    public void OpenMainCharacterSelection()
    {
        isSelectingMain = true;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // ������ ĳ���� ���á� ��ư
    public void OpenSubCharacterSelection()
    {
        if (selectedMain == null)
        {
            Debug.Log("���� ĳ���͸� ���� �����ϼ���.");
            return;
        }
        isSelectingMain = false;
        RefreshButtons();
        characterButtonPanel.SetActive(true);
    }

    // ��Ͽ��� ĳ���� �ϳ��� ������ �� (�� ��ư OnClick�� index �Ҵ�)
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
                Debug.Log("���ΰ� ���� ĳ���ʹ� ����� ������ �� �����ϴ�.");
                return;
            }
            selectedSub = index;
        }

        // ���� �� �г� �ݱ� (���ϸ� �����ص� ��)
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
        SceneManager.LoadScene("CharacterSelect");
    }

    // ���� ���� �� ���ΰ� ������ ��ư�� ��Ȱ��ȭ
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