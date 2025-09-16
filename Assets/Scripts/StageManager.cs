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

    // 외부(UI 버튼)에서 호출
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
        // 씬 로드
        var op = SceneManager.LoadSceneAsync(data.sceneName, LoadSceneMode.Single);
        yield return op;

        CurrentStage = data;

        // 로드가 끝난 뒤 환경 적용

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
        // 씬 안의 모든 EnemySpawner에 현재 스테이지 설정 주입
    //    var spawners = FindObjectsOfType<EnemySpawner>(true);
    //    foreach (var s in spawners)
    //    {
    //        s.Configure(CurrentStage);
    //    }
    //}
}
