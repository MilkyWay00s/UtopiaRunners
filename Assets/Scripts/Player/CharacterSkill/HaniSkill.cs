using UnityEngine;

public class HaniSkill : MonoBehaviour
{
    [Header("Gauge")]
    public int maxGauge = 5;          // 게이지 칸 수
    private int currentGauge = 0;

    [Header("Shock Effect")]
    public float shockDuration = 10f;


    public int extraJumpCount = 1;
    bool empoweredAttackReady = false;

    void OnEnable()
    {
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.maxJumpCount += extraJumpCount;
        }
    }

    void OnDisable()
    {
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.maxJumpCount -= extraJumpCount;
        }

        //하니에서 다른 캐릭터로 변경시 게이지 초기화
        //currentGauge = 0;
        //empoweredAttackReady = false;
    }

    //2단 / 3단 점프시 게이지 충전
    public void OnAirJump(int jumpCount)
    {
        // 2단, 3단 점프일 때만 충전
        if (jumpCount < 2) return;

        if (currentGauge >= maxGauge) return;

        currentGauge++;

        if (currentGauge >= maxGauge)
        {
            empoweredAttackReady = true;
        }
    }

    //게이지가 찬 후 기본 공격 적중 시
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