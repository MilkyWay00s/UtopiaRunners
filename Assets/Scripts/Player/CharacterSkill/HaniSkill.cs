using UnityEngine;

public class HaniSkill : MonoBehaviour
{
    [Header("Gauge")]
    public int maxGauge = 5;
    private int currentGauge = 0;

    [Header("Shock Effect")]
    public float shockDuration = 10f;

    public int extraJumpCount = 1;
    bool empoweredAttackReady = false;

    [SerializeField] private Animator animator;

    PlayerController pc;

    void OnEnable()
    {
        pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.maxJumpCount += extraJumpCount;
            pc.OnJumped += HandleJumped;
        }

        if (animator) animator.SetInteger("RunnerIdx", 2);
    }

    void OnDisable()
    {
        if (pc != null)
        {
            pc.OnJumped -= HandleJumped;
            pc.maxJumpCount -= extraJumpCount;
        }

        // 캐릭터 교체 시 게이지 초기화
        // currentGauge = 0;
        // empoweredAttackReady = false;
    }

    void HandleJumped(int jumpCount)
    {
        // 2단, 3단 점프일 때만 충전
        if (jumpCount < 2) return;
        if (currentGauge >= maxGauge) return;

        currentGauge++;

        if (currentGauge >= maxGauge)
            empoweredAttackReady = true;
    }

    public void OnBasicAttackHit(EnemyCondition enemy)
    {
        if (!empoweredAttackReady) return;
        if (enemy == null) return;

        enemy.ApplyStun(shockDuration);

        empoweredAttackReady = false;
        currentGauge = 0;
    }

    public bool IsGaugeFull() => empoweredAttackReady;
}