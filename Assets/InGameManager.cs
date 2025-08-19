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

    void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        if (gameOverPanel) gameOverPanel.SetActive(false);

        timeLeft = timeLimit;
        UpdateTimerUI();

        // CharacterManager ã�� �̺�Ʈ ����
        cm = FindObjectOfType<CharacterManager>();
        if (cm != null)
        {
            cm.OnActiveCharacterDeath += GameOver; // Ȱ�� ĳ���� ��� �� ���ӿ���
        }
        else
        {
            Debug.LogError("[GameManager] CharacterManager�� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        if (isGameOver) return;

        timeLeft -= Time.deltaTime;
        UpdateTimerUI();
        if (timeLeft <= 0f) GameOver();
    }

    private void UpdateTimerUI()
    {
        if (timerText)
            timerText.text = $"Time: {Mathf.CeilToInt(Mathf.Max(0f, timeLeft))}";
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;
        if (gameOverPanel) gameOverPanel.SetActive(true);
        Debug.Log("[GameManager] Game Over");
    }

    // (����) �ٽ� ����/�޴��� ���� ��ư���� ȣ���� �� �ִ� ����
    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}