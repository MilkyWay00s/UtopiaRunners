using System.Collections;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectCotroller : MonoBehaviour
{
    //----------------------------------------------------------------------------
    [Header("Stage Nodes (Index Order)")]
    public StageNode[] stages;
    [SerializeField] private Sprite[] stageNodeSprites;

    [Header("Pointer")]
    public Transform pointer;

    [Header("UI")]
    public TMP_Text stageNameText;
    public TMP_Text stageDescText;

    [Header("Scene Names")]
    [SerializeField] private string characterSelectSceneName;
    [SerializeField] private bool goCharacterSelectBeforeRunning = true;

    private int currentIndex = 0;
    private bool isCurrentStageUnlocked = true;
    void Start()
    {
        if (stages == null || stages.Length == 0) return;

        int lastStageNumber = 1;
        if (GameManager.Instance != null)
            lastStageNumber = GameManager.Instance.GetLastEnteredStageNumber();

        int startIndex = lastStageNumber - 1;
        startIndex = Mathf.Clamp(startIndex, 0, stages.Length - 1);

        startIndex = ClampToUnlockedIndex(startIndex);

        SetIndex(startIndex);
    }
    void Update()
    {
        if (stages == null || stages.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int next = currentIndex + 1;
            if (next >= stages.Length) next = 0;
            SetIndex(next);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int prev = currentIndex - 1;
            if (prev < 0) prev = stages.Length - 1;
            SetIndex(prev);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }
    }
    int ClampToUnlockedIndex(int desiredIndex)
    {
        if (GameManager.Instance == null) return 0;

        string worldName = GameManager.Instance.currentWorld;

        int idx = desiredIndex;
        while (idx > 0)
        {
            int stageNumber = idx + 1;
            bool unlocked = GameManager.Instance.IsStageUnlocked(worldName, stageNumber);
            if (unlocked) return idx;

            idx--;
        }
        return 0;
    }
    void SetIndex(int index)
    {
        currentIndex = index;
        var node = stages[currentIndex];
        if (node == null) return;
        int stageNumber = currentIndex + 1;
        if (GameManager.Instance != null)
        {
            string worldName = GameManager.Instance.currentWorld; // 예: "World1"
            isCurrentStageUnlocked = GameManager.Instance.IsStageUnlocked(worldName, stageNumber);
        }
        else
        {
            isCurrentStageUnlocked = false;
        }

        if (stageNameText) stageNameText.text = node.stageTitle;
        if (stageDescText)
        {
            if (isCurrentStageUnlocked)
                stageDescText.text = node.stageDesc;
            else
                stageDescText.text = node.stageDesc + "\n(잠김: 이전 스테이지 클리어 필요)";
        }

        if (pointer && node.pointerAnchor)
            pointer.position = node.pointerAnchor.position;

        if (isCurrentStageUnlocked && GameManager.Instance != null)
        {
            GameManager.Instance.SelectStage(node.stageId, save: false);
        }
    }
    public void ConfirmSelection()
    {
        if (!isCurrentStageUnlocked)
        {
            Debug.Log("이전 스테이지를 클리어해야 선택할 수 있습니다.");
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentStage = $"Stage{currentIndex + 1}";
            GameManager.Instance.SaveGame(GameManager.Instance.currentSlot);
        }

        if (goCharacterSelectBeforeRunning)
            SceneManager.LoadScene(characterSelectSceneName);
        else
            SceneManager.LoadScene("RunningScene");
    }
}