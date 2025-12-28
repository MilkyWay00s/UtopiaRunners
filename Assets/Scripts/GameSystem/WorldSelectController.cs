using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldSelectController : MonoBehaviour
{
    //public Camera cam;
    public TMP_Text worldNameText;
    public TMP_Text worldDescriptionText;

    [Header("World List")]
    public WorldCollider[] worlds;

    [Header("Pointer")]
    public Transform pointer;

    [Header("Unlock Rule")]
    public int stagesPerWorld = 6;

    private int currentIndex = 0;
    private string selectedWorldSceneName;
    private bool isCurrentWorldUnlocked = true;

    void Start()
    {
        if (worlds == null || worlds.Length == 0)
        {
            Debug.LogWarning("WorldManager: worlds 배열이 비어 있습니다.");
            return;
        }
        RefreshLocks();
        SetSelectedWorld(0);
    }
    void RefreshLocks()
    {
        if (GameManager.Instance == null) return;

        foreach (var w in worlds)
        {
            if (w == null || w.lockIcon == null) continue;
            bool unlocked = GameManager.Instance.IsWorldUnlocked((int)w.worldID, stagesPerWorld);
            w.lockIcon.SetActive(!unlocked);
        }
    }
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                WorldCollider area = hit.collider.GetComponent<WorldCollider>();
                if (area != null)
                {
                    worldNameText.text = area.worldName;
                    worldDescriptionText.text = area.worldDescription;
                    selectedWorldSceneName = area.sceneName;
                }
            }
        }*/
        if (worlds == null || worlds.Length == 0) return;

        // 좌/우 방향키 인덱스
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int next = currentIndex + 1;
            if (next >= worlds.Length) next = 0;           // 끝이면 처음으로
            SetSelectedWorld(next);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int prev = currentIndex - 1;
            if (prev < 0) prev = worlds.Length - 1;        // 처음이면 끝으로
            SetSelectedWorld(prev);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            onGoButtonClicked();
        }
    }
    private void SetSelectedWorld(int index)
    {
        currentIndex = index;

        WorldCollider area = worlds[currentIndex];
        if (area == null) return;

        // 텍스트 갱신
        worldNameText.text = area.worldName;
        if (GameManager.Instance != null)
        {
            isCurrentWorldUnlocked =
                GameManager.Instance.IsWorldUnlocked((int)area.worldID, stagesPerWorld);
        }
        else
        {
            isCurrentWorldUnlocked = false;
        }
        if (isCurrentWorldUnlocked)
            worldDescriptionText.text = area.worldDescription;
        else
            worldDescriptionText.text = area.worldDescription + "\n(잠김: 이전 월드 마지막 스테이지 클리어 필요)";
        selectedWorldSceneName = area.sceneName;

        // 포인터 위치 이동
        if (pointer != null)
        {
            if (area.pointerAnchor != null)
            {
                pointer.position = area.pointerAnchor.position;
            }
        }
    }
    public void onGoButtonClicked()
    {
        if (!isCurrentWorldUnlocked)
        {
            Debug.Log("이전 월드의 마지막 스테이지를 클리어해야 진입할 수 있습니다.");
            return;
        }

        if (!string.IsNullOrEmpty(selectedWorldSceneName))
        {
            SceneManager.LoadScene(selectedWorldSceneName);
        }
    }
}
