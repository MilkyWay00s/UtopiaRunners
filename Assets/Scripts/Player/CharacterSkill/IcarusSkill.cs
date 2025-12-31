using UnityEngine;
using System.Collections;

public class IcarusSkill : MonoBehaviour
{
    [Header("Cooldown Reduce")]
    public float cooldownReduceAmount = 1.0f;

    [Header("Tag Dash")]
    public float dashSpeed = 20f;
    public float dashStopDistance = 0.3f;

    WeaponSkill weaponSkill;
    bool isDashing = false;
    [SerializeField] private Animator animator;
    void Awake()
    {
        // Player에 붙은 WeaponSkill 찾기
        weaponSkill = FindObjectOfType<WeaponSkill>();
    }

    public void OnJumpOrSlide()
    {
        if (weaponSkill == null) return;

        weaponSkill.ReduceCooldown(cooldownReduceAmount);
    }

    public void OnTagEnter()
    {
        if (isDashing) return;
        StartCoroutine(TagDash());
    }

    IEnumerator TagDash()
    {
        isDashing = true;

        Transform target = FindNearestEnemy();
        if (target == null)
        {
            isDashing = false;
            yield break;
        }

        Vector3 originPos = transform.position;

        // 태그 후 돌진
        while (Vector3.Distance(transform.position, target.position) > dashStopDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }

        // 데미지/히트 판정 추가 가능

        yield return new WaitForSeconds(0.05f);

        while (Vector3.Distance(transform.position, originPos) > dashStopDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                originPos,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }

        isDashing = false;
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Boss");

        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (GameObject e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }
        return nearest;
    }
}