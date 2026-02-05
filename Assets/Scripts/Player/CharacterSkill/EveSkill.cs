using UnityEngine;

public class EveSkill : MonoBehaviour
    
{
    [Header("Shield Condition")]
    public float noDamageTime = 5f;      // 피해 안 받는 시간

    [Header("Attack Buff")]
    public float attackBonusRate = 0.2f; // 공격력 +20%

    [Header("SkillLevel Scaling")]
    public int characterIndex = 0;          // Eve 인덱스(필요하면 Inspector에서 설정)
    public float baseShieldDuration = 3f;   // 보호막 기본 지속시간
    public float shieldDurationPerLevel = 0.5f; // 레벨당 지속시간 증가
    public float baseAttackBonusRate = 0.2f;    // 보호막 공격버프 기본값
    public float attackBonusPerLevel = 0.05f;   // 레벨당 공격버프 증가

    [Header("Time Without Buff")]
    float timer = 0f;

    bool shieldActive = false;

    float shieldTimer = 0f;
    float shieldDuration = 0f;

    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        if (animator) animator.SetInteger("RunnerIdx", 1);

        ApplySkillLevel();
    }
    void Update()
    {
        // 보호막이 없을 때만 타이머 증가
        if (shieldActive)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= shieldDuration)
            {
                // 지속시간 끝나면 보호막 해제
                shieldActive = false;
                ApplyAttackBuff(false);
            }
            return;
        }

        timer += Time.deltaTime;

        if (timer >= noDamageTime)
        {
            ActivateShield();
        }
    }

    // 보호막 생성

    void ApplySkillLevel()
    {
        int lv = UpgradeState.GetLevel($"C{characterIndex}", UpgradeType.SkillLevel);
        float bonus = UpgradeRules.GetSkillLevelBonus(lv);

        // 보호막 지속시간 강화
        shieldDuration = baseShieldDuration + shieldDurationPerLevel * bonus;

        // 공격버프 비율 강화
        attackBonusRate = baseAttackBonusRate + attackBonusPerLevel * bonus;

        // 안전장치
        if (shieldDuration < 0f) shieldDuration = 0f;
        if (attackBonusRate < 0f) attackBonusRate = 0f;
    }

    void ActivateShield()
    {
        shieldActive = true;
        timer = 0f;
        shieldTimer = 0f;


        ApplyAttackBuff(true);
    }

    public void ModifyDamage(ref int damage)
    {
        if (damage <= 0) return;

        // if 피격시도,  타이머 리셋 
        timer = 0f;

        // if 쉴드,  데미지 0 && 쉴드 파괴
        if (shieldActive)
        {
            BreakShield();
            damage = 0;  
        }
    }

    void BreakShield()
    {
        shieldActive = false;
        shieldTimer = 0f;
        ApplyAttackBuff(false);

        Debug.Log("Eve Shield Broken");
    }

    //공격력 증가 / 해제
    void ApplyAttackBuff(bool apply)
    {
        var weapon = GetComponentInChildren<WeaponAutoAttack>();
        if (weapon == null) return;

        /*if (apply)
            weapon.attackMultiplier += attackBonusRate;
        else
            weapon.attackMultiplier -= attackBonusRate;*/
    }

    public bool IsShieldActive() => shieldActive;
}