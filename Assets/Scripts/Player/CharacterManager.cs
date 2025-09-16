using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class CharacterManager : MonoBehaviour
{
    [Header("Spawn")]
    public CharacterDatabase characterDatabase;   // A~D �����յ�
    public Transform spawnPoint;

    [Header("Switching")]
    public KeyCode switchKey = KeyCode.Q;
    public float switchCooldown = 15f;
    private float switchRemain = 0f;

    [Header("HP UI (�»��)")]
    public TMP_Text activeHpLabel;   // "Active HP"
    public Slider activeHpBar;
    public TMP_Text reserveHpLabel;  // "Reserve HP"
    public Slider reserveHpBar;

    // �ܺ�(GameManager)�� ������ �̺�Ʈ
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
            Debug.LogError("[CM] ������ ���Կ� ����ִ� ��Ұ� �ֽ��ϴ�."); return;
        }

        // --- ���� ---
        Vector3 pos = spawnPoint ? spawnPoint.position : Vector3.zero;
        pos.z = 0f; // 2D ī�޶� ����
        mainObj = Instantiate(characterDatabase.allCharacters[mainIndex].runnerObj, pos, Quaternion.identity);
        subObj = Instantiate(characterDatabase.allCharacters[subIndex].runnerObj, pos, Quaternion.identity);

        // ó���� ���θ� ���̰�
        mainObj.SetActive(true);
        subObj.SetActive(true);
        activeObj = mainObj;
        reserveObj = subObj;
        reserveObj.SetActive(false);

        BindHPRefs();   // HP �̺�Ʈ/UI ���ε�
    }

    void Update()
    {
        if (switchRemain > 0f) switchRemain -= Time.deltaTime;
        if (Input.GetKeyDown(switchKey) && switchRemain <= 0f)
            SwitchCharacters();
    }

    private void BindHPRefs()
    {
        // ���� ���� ����(�ߺ� ����)
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

        // �̺�Ʈ �籸��
        activeHP.OnDeath += HandleActiveDeath;
        activeHP.OnHealthChanged += OnActiveHpChanged;
        reserveHP.OnHealthChanged += OnReserveHpChanged;

        // UI �ʱ�ȭ
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
        OnActiveCharacterDeath?.Invoke(); // ���� ���� ĳ���� ��� �� GameManager�� �˸�
    }

    private void SwitchCharacters()
    {
        // ���� Ȱ�� ĳ������ ��ġ(�׸��� �ӵ�) �̾�ޱ�
        Vector3 pos = activeObj.transform.position;
        Vector2 velFrom = Vector2.zero;
        var rbFrom = activeObj.GetComponent<Rigidbody2D>();
        if (rbFrom) velFrom = rbFrom.velocity;

        // ��Ȱ��ȭ/Ȱ��ȭ + ��ġ ����ȭ(�� ȭ�鿡 �ϳ��� ���̵���)
        activeObj.SetActive(false);               // �ݵ�� ��Ʈ ������Ʈ�� ȣ��
        reserveObj.transform.position = pos;      // ���� ��ġ�� �̵�
        reserveObj.SetActive(true);

        // �ӵ��� �̾��ֱ�(����)
        var rbTo = reserveObj.GetComponent<Rigidbody2D>();
        if (rbFrom && rbTo) rbTo.velocity = velFrom;

        // ���� ���� �� UI/�̺�Ʈ ����ε�
        var tmp = activeObj; activeObj = reserveObj; reserveObj = tmp;
        BindHPRefs();

        switchRemain = switchCooldown;
    }
}