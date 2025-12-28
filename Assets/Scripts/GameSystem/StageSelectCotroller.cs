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
        SetIndex(0);
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
    void SetIndex(int index)
    {
        currentIndex = index;
        var node = stages[currentIndex];
        if (node == null) return;

        if (GameManager.Instance != null)
        {
            string worldName = GameManager.Instance.currentWorld; // 예: "World1"
            isCurrentStageUnlocked = GameManager.Instance.IsStageUnlocked(worldName, currentIndex);
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
            GameManager.Instance.SelectStage(node.stageId, save: true);
    }
    public void ConfirmSelection()
    {
        if (!isCurrentStageUnlocked)
        {
            Debug.Log("이전 스테이지를 클리어해야 선택할 수 있습니다.");
            return;
        }

        if (goCharacterSelectBeforeRunning)
        {
            SceneManager.LoadScene(characterSelectSceneName);
        }
        else
        {
            SceneManager.LoadScene("RunningScene");
        }
    }
}