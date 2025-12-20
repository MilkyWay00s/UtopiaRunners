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

        if (stageNameText) stageNameText.text = node.stageTitle;
        if (stageDescText) stageDescText.text = node.stageDesc;

        if (pointer && node.pointerAnchor)
            pointer.position = node.pointerAnchor.position;

        if (GameManager.Instance != null)
            GameManager.Instance.SelectStage(node.stageId, save: true);
    }
    public void ConfirmSelection()
    {
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