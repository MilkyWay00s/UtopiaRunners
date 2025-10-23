using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("Database")]
    public StageDatabase database;

    [Header("Common Scene")]
    [SerializeField] private string runningSceneName = "RunningScene"; 

    [Header("State (runtime)")]
    [SerializeField] private StageData currentStage;  
    public StageData CurrentStage => currentStage;

    const string PREF_LAST_STAGE = "LastStageId";

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnAnySceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnAnySceneLoaded;
    }

    void OnAnySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // RunningScene�� �ε�Ǹ� ���õ� StageData�� ���� ����
        if (scene.name == runningSceneName && currentStage != null)
        {
            ApplyEnvironment();
            //ApplyAudio();
            ApplyPlayerStart();
            //ApplyToSpawners();
        }
    }

    // ����� UI���� ȣ��: ID ���� �� ���� RunningScene �ε�
    public void SelectAndPlay(StageName id, bool remember = true)
    {
        var data = database?.GetStageName(id);
        if (data == null) { Debug.LogError($"StageData not found: {id}"); return; }
        currentStage = data;
        if (remember) PlayerPrefs.SetInt(PREF_LAST_STAGE, (int)id);

    
        if (SceneManager.GetActiveScene().name == runningSceneName)
        {
            ApplyEnvironment();
            //ApplyAudio();
            ApplyPlayerStart();
            //ApplyToSpawners();
        }
        else
        {
            StartCoroutine(LoadRunningScene());
        }
    }

    public void LoadLastSelectedOr(StageName fallback)
    {
        var saved = PlayerPrefs.GetInt(PREF_LAST_STAGE, (int)fallback);
        SelectAndPlay((StageName)saved, remember: false);
    }

    IEnumerator LoadRunningScene()
    {
        var op = SceneManager.LoadSceneAsync(runningSceneName, LoadSceneMode.Single);
        yield return op; // �ε� �Ϸ� �� OnAnySceneLoaded���� �ڵ� ����
    }


    void ApplyEnvironment()
    {
        RenderSettings.ambientLight = currentStage.ambientLight;

        var bg = GameObject.Find("Background");
        if (bg && currentStage.background)
        {
            var sr = bg.GetComponent<SpriteRenderer>();
            if (sr) sr.sprite = currentStage.background;
        }
    }

    /*void ApplyAudio()
    {
        var go = GameObject.Find("BgmPlayer");
        if (!go) return;
        var src = go.GetComponent<AudioSource>();
        if (!src) return;
        src.clip = currentStage.bgm;
        src.volume = currentStage.bgmVolume;
        if (src.clip) src.Play(); else src.Stop();
    }
    */

    void ApplyPlayerStart()
    {
        var player = GameObject.FindWithTag("Player");
        if (player) player.transform.position = (Vector3)currentStage.playerStartPos;
    }

    //void ApplyToSpawners()
    //{
    //    var spawners = Object.FindObjectsOfType<EnemySpawner>(true);
    //    foreach (var s in spawners) s.Configure(currentStage);
    //}
}