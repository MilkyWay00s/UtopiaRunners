using UnityEngine;

public class EveSkill : MonoBehaviour
{
    [Header("Shield Condition")]
    public float noDamageTime = 5f;      // 피해 안 받는 시간

    [Header("Attack Buff")]
    public float attackBonusRate = 0.2f; // 공격력 +20%

    [Header("Time Without Buff")]
    float timer = 0f;

    bool shieldActive = false;

    void Update()
    {
        // 보호막이 없을 때만 타이머 증가
        if (shieldActive) return;

        timer += Time.deltaTime;

        if (timer >= noDamageTime)
        {
            ActivateShield();
        }
    }

    // 보호막 생성
    void ActivateShield()
    {
        shieldActive = true;
        timer = 0f;

        ApplyAttackBuff(true);
    }

    // 피해를 받았을 때 (1회 피격)
    public void OnDamaged()
    {
        timer = 0f;

        if (!shieldActive) return;

        BreakShield();
    }

    void BreakShield()
    {
        shieldActive = false;
        ApplyAttackBuff(false);

        Debug.Log("Eve Shield Broken");
    }

    //공격력 증가 / 해제
    void ApplyAttackBuff(bool apply)
    {
        var weapon = GetComponentInChildren<WeaponAutoAttack>();
        if (weapon == null) return;

        if (apply)
            weapon.attackMultiplier += attackBonusRate;
        else
            weapon.attackMultiplier -= attackBonusRate;
    }

    public bool IsShieldActive() => shieldActive;
}