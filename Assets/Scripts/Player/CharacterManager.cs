using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class CharacterManager : MonoBehaviour
{
    [Header("Spawn")]
    public CharacterDatabase characterDatabase;   // A~D 프리팹들
    public Transform spawnPoint;

    [Header("Switching")]
    public KeyCode switchKey = KeyCode.Q;
    public float switchCooldown = 15f;
    private float switchRemain = 0f;

    [Header("HP UI (좌상단)")]
    public TMP_Text activeHpLabel;   // "Active HP"
    public Slider activeHpBar;
    public TMP_Text reserveHpLabel;  // "Reserve HP"
    public Slider reserveHpBar;

    // 외부(GameManager)가 구독할 이벤트
    public event Action OnActiveCharacterDeath;

    private GameObject mainObj, subObj;
    private GameObject activeObj, reserveObj;
    private PlayerHealth activeHP, reserveHP;

    public PlayerHealth ActiveHP => activeHP;
    public PlayerHealth ReserveHP => reserveHP;

    void Start()
    {
        int mainIndex = PlayerPrefs.GetInt("MainCharacter", 0);
        int subIndex = PlayerPrefs.GetInt("SubCharacter", 1);

        if (!characterDatabase.allCharacters[mainIndex] || !characterDatabase.allCharacters[subIndex])
        {
            Debug.LogError("[CM] 프리팹 슬롯에 비어있는 요소가 있습니다."); return;
        }

        // --- 스폰 ---
        Vector3 pos = spawnPoint ? spawnPoint.position : Vector3.zero;
        pos.z = 0f; // 2D 카메라 가시
        mainObj = Instantiate(characterDatabase.allCharacters[mainIndex].runnerObj, pos, Quaternion.identity);
        subObj = Instantiate(characterDatabase.allCharacters[subIndex].runnerObj, pos, Quaternion.identity);

        // 처음엔 메인만 보이게
        mainObj.SetActive(true);
        subObj.SetActive(true);
        activeObj = mainObj;
        reserveObj = subObj;
        reserveObj.SetActive(false);

        BindHPRefs();   // HP 이벤트/UI 바인딩
    }

    void Update()
    {
        if (switchRemain > 0f) switchRemain -= Time.deltaTime;
        if (Input.GetKeyDown(switchKey) && switchRemain <= 0f)
            SwitchCharacters();
    }

    private void BindHPRefs()
    {
        // 기존 구독 해제(중복 방지)
        if (activeHP != null)
        {
            activeHP.OnDeath -= HandleActiveDeath;
            activeHP.OnHealthChanged -= OnActiveHpChanged;
        }
        if (reserveHP != null)
        {
            reserveHP.OnHealthChanged -= OnReserveHpChanged;
        }

        activeHP = activeObj.GetComponent<PlayerHealth>();
        reserveHP = reserveObj.GetComponent<PlayerHealth>();

        // 이벤트 재구독
        activeHP.OnDeath += HandleActiveDeath;
        activeHP.OnHealthChanged += OnActiveHpChanged;
        reserveHP.OnHealthChanged += OnReserveHpChanged;

        // UI 초기화
        if (activeHpLabel) activeHpLabel.text = "Active HP";
        if (reserveHpLabel) reserveHpLabel.text = "Reserve HP";

        if (activeHpBar) { activeHpBar.maxValue = activeHP.maxHealth; activeHpBar.value = activeHP.CurrentHealth; }
        if (reserveHpBar) { reserveHpBar.maxValue = reserveHP.maxHealth; reserveHpBar.value = reserveHP.CurrentHealth; }
    }

    private void OnActiveHpChanged(int cur, int max)
    {
        if (activeHpBar) { activeHpBar.maxValue = max; activeHpBar.value = cur; }
    }

    private void OnReserveHpChanged(int cur, int max)
    {
        if (reserveHpBar) { reserveHpBar.maxValue = max; reserveHpBar.value = cur; }
    }

    private void HandleActiveDeath()
    {
        OnActiveCharacterDeath?.Invoke(); // 현재 조종 캐릭터 사망 → GameManager에 알림
    }

    private void SwitchCharacters()
    {
        // 현재 활성 캐릭터의 위치(그리고 속도) 이어받기
        Vector3 pos = activeObj.transform.position;
        Vector2 velFrom = Vector2.zero;
        var rbFrom = activeObj.GetComponent<Rigidbody2D>();
        if (rbFrom) velFrom = rbFrom.velocity;

        // 비활성화/활성화 + 위치 동기화(한 화면에 하나만 보이도록)
        activeObj.SetActive(false);               // 반드시 루트 오브젝트에 호출
        reserveObj.transform.position = pos;      // 같은 위치로 이동
        reserveObj.SetActive(true);

        // 속도도 이어주기(선택)
        var rbTo = reserveObj.GetComponent<Rigidbody2D>();
        if (rbFrom && rbTo) rbTo.velocity = velFrom;

        // 참조 스왑 및 UI/이벤트 재바인딩
        var tmp = activeObj; activeObj = reserveObj; reserveObj = tmp;
        BindHPRefs();

        switchRemain = switchCooldown;
    }
}