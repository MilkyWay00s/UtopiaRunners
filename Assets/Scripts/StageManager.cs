using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("Database")]
    public StageDatabase database;

    public StageData CurrentStage { get; private set; }

    const string PREF_LAST_STAGE = "LastStageId";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (database != null) database.BuildMap();
    }

    // �ܺ�(UI ��ư)���� ȣ��
    public void SelectAndLoad(StageName id)
    {
        var data = database?.GetStageId(id);
        if (data == null)
        {
            Debug.LogError($"StageManager: StageData not found for {id}");
            return;
        }
        PlayerPrefs.SetInt(PREF_LAST_STAGE, (int)id);
        StartCoroutine(LoadStageRoutine(data));
    }

    public void LoadLastSelectedOr(StageName fallback)
    {
        var saved = PlayerPrefs.GetInt(PREF_LAST_STAGE, (int)fallback);
        SelectAndLoad((StageName)saved);
    }

    private IEnumerator LoadStageRoutine(StageData data)
    {
        // �� �ε�
        var op = SceneManager.LoadSceneAsync(data.sceneName, LoadSceneMode.Single);
        yield return op;

        CurrentStage = data;

        // �ε尡 ���� �� ȯ�� ����

        ApplyPlayerStart();
        //ApplyToSpawners();

        Debug.Log($"[StageManager] Loaded: {data.displayName}");
    }

    private void ApplyPlayerStart()
    {
        var player = GameObject.FindWithTag("Player");
        if (player)
        {
            player.transform.position = (Vector3)CurrentStage.playerStartPos;
        }
    }

    //private void ApplyToSpawners()
    //{
        // �� ���� ��� EnemySpawner�� ���� �������� ���� ����
    //    var spawners = FindObjectsOfType<EnemySpawner>(true);
    //    foreach (var s in spawners)
    //    {
    //        s.Configure(CurrentStage);
    //    }
    //}
}
