using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapPointer : MonoBehaviour
{
    public string[] sceneNames;
    public TextMeshProUGUI CurrentWorldText;
    public Transform[] regionPositions;
    public Transform pointer;

    private int currentIndex = 0;

    void Start()
    {
        MovePointer(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePointer(currentIndex + 1);
            updateWorldName();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePointer(currentIndex - 1);
            updateWorldName();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePointer(currentIndex - 1);
            updateWorldName();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePointer(currentIndex + 1);
            updateWorldName();
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // Enter 키 입력
        {
            LoadCurrentScene();
        }
    }

    void MovePointer(int newIndex)
    {
        int count = regionPositions.Length;

        if (count == 0) return;

        currentIndex = (newIndex + count) % count;

        if (regionPositions.Length > 0)
        {
            Transform target = regionPositions[currentIndex];
            SpriteRenderer sr = target.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                // 스프라이트의 중심 위치 (local)
                Vector3 center = target.position;

                // 스프라이트 높이 구하기 (world 단위)
                float spriteHeight = sr.bounds.size.y;

                // 위쪽 중앙 위치 = 스프라이트 중심 + 높이의 절반 만큼 위로 이동
                Vector3 topCenter = center + new Vector3(0, spriteHeight / 2f, 0);

                pointer.position = topCenter;
            }
        }
    }

    void LoadCurrentScene()
    {
        if (sceneNames.Length == regionPositions.Length && currentIndex < sceneNames.Length)
        {
            string sceneToLoad = sceneNames[currentIndex];
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("씬 이름이 비어 있습니다.");
            }
        }
        else
        {
            Debug.LogWarning("씬 이름 배열과 지역 배열의 길이가 다르거나 인덱스 초과입니다.");
        }
    }

    void updateWorldName()
    {
        CurrentWorldText.text = regionPositions[currentIndex].name;
    }
}
