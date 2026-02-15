using System.Collections.Generic;
using UnityEngine;

public class HaniSkill : MonoBehaviour
{
    [Header("Gauge")]
    public int maxGauge = 5;
    private float currentGauge = 0f;

    [Header("Shock Effect")]
    public float shockDuration = 10f;

    public int extraJumpCount = 1;
    bool empoweredAttackReady = false;

    [SerializeField] private Animator animator;

    PlayerController pc;

    public int characterIndex = 2;

    
    float gaugeGainPerAirJump = 1f;   // 기본: 1칸
    int shockTargetCount = 1;      // 기본: 1명(맞은 적)

        public float multiShockRange = 6f;

    void OnEnable()
    {
        ApplySkillLevel();

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

    void ApplySkillLevel()
    {
        int lv = UpgradeState.GetLevel($"C{characterIndex}", UpgradeType.SkillLevel);
        float bonus = UpgradeRules.GetSkillLevelBonus(lv); // 너 규칙: level 그대로 반환

        int b = Mathf.FloorToInt(bonus);

        // 강화 요소 1) 2단 점프 시 차는 게이지 양
        gaugeGainPerAirJump = 1f + b*0.2f;   // Lv0=1, Lv1=2, Lv2=3 ...

        // 강화 요소 2) 풀차지 시 감전되는 적 수
        shockTargetCount = 1 + b;      // Lv0=1명, Lv1=2명, Lv2=3명 ...

        if (gaugeGainPerAirJump < 1) gaugeGainPerAirJump = 1;
        if (shockTargetCount < 1) shockTargetCount = 1;
    }


    void HandleJumped(int jumpCount)
    {
        // 2단, 3단 점프일 때만 충전
        if (jumpCount < 2) return;
        if (currentGauge >= maxGauge) return;

        currentGauge = Mathf.Min(currentGauge + gaugeGainPerAirJump, maxGauge);

        if (currentGauge >= maxGauge)
            empoweredAttackReady = true;
    }

    public void OnBasicAttackHit(EnemyCondition enemy)
    {
        if (!empoweredAttackReady) return;
        if (enemy == null) return;

        enemy.ApplyStun(shockDuration);

       int  needExtra = shockTargetCount - 1;
        if (needExtra > 0)
        {
            ApplyMultiShock(enemy, needExtra);
        }

        empoweredAttackReady = false;
        currentGauge = 0;
    }

    void ApplyMultiShock(EnemyCondition firstTarget, int extraCount)
    {
        // EnemyCondition을 씬에서 찾아 거리순으로 추가 적용
        EnemyCondition[] all = GameObject.FindObjectsOfType<EnemyCondition>();

        Vector3 origin = firstTarget.transform.position;

        // firstTarget 제외 + 범위 내만 모으기
        List<EnemyCondition> candidates = new List<EnemyCondition>();
        for (int i = 0; i < all.Length; i++)
        {
            var e = all[i];
            if (e == null) continue;
            if (e == firstTarget) continue;

            float d = Vector3.Distance(origin, e.transform.position);
            if (d <= multiShockRange)
                candidates.Add(e);
        }

        // 거리순 정렬
        candidates.Sort((a, b) =>
            Vector3.Distance(origin, a.transform.position)
            .CompareTo(Vector3.Distance(origin, b.transform.position))
        );

        // extraCount명에게 추가 스턴
        for (int i = 0; i < candidates.Count && extraCount > 0; i++)
        {
            candidates[i].ApplyStun(shockDuration);
            extraCount--;
        }
    }
    public bool IsGaugeFull() => empoweredAttackReady;
}