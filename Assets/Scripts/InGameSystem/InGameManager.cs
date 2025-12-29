using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InGameManager : MonoBehaviour
{
    [Header("Timer & Game Over")]
    public float timeLimit = 60f;
    public TMP_Text timerText;
    public GameObject gameOverPanel;

    private float timeLeft;
    private CharacterManager cm;
    private bool isGameOver = false;
    public StageData2 stageData;

    void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        if (gameOverPanel) gameOverPanel.SetActive(false);

        timeLeft = timeLimit;
        UpdateTimerUI();

        // CharacterManager 찾고 이벤트 구독
        cm = FindObjectOfType<CharacterManager>();
        /*
        if (cm != null)
        {
            cm.OnActiveCharacterDeath += GameOver; // 활성 캐릭터 사망 시 게임오버
        }
        else
        {
            Debug.LogError("[GameManager] CharacterManager를 찾을 수 없습니다.");
        }
        */
    }

    void Update()
    {
        if (isGameOver) return;

        timeLeft -= Time.deltaTime;
        UpdateTimerUI();
        if (timeLeft <= 0f) CommonStageClear();
    }

    private void UpdateTimerUI()
    {
        if (timerText)
            timerText.text = $"Time: {Mathf.CeilToInt(Mathf.Max(0f, timeLeft))}";
    }

    public void CommonStageClear()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;
        if (gameOverPanel) gameOverPanel.SetActive(true);


        GameManager.Instance.coin += stageData.stageRewardCoin;
        GameManager.Instance.SaveGame(GameManager.Instance.currentSlot);
    }


    // (선택) 다시 시작/메뉴로 가기 버튼에서 호출할 수 있는 헬퍼
    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onBackButtonClicked()
    {
        SceneManager.LoadScene("3_CharacterSelect");
    }
}