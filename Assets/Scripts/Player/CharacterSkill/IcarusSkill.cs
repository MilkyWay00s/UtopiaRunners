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
    PlayerController pc;
    CharacterManager cm;

    bool isDashing = false;

    void Awake()
    {
        weaponSkill = GetComponentInChildren<WeaponSkill>();
        pc = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        if (pc != null)
        {
            pc.OnJumped += HandleJumped;
            pc.OnSlideStarted += HandleSlideStarted;
        }

        cm = FindObjectOfType<CharacterManager>();
        if (cm != null)
            cm.OnTagSwitched += HandleTagSwitched;
    }

    void OnDisable()
    {
        if (pc != null)
        {
            pc.OnJumped -= HandleJumped;
            pc.OnSlideStarted -= HandleSlideStarted;
        }

        if (cm != null)
            cm.OnTagSwitched -= HandleTagSwitched;
    }

    void HandleJumped(int jumpCount)
    {
        ReduceCooldown();
    }

    void HandleSlideStarted()
    {
        ReduceCooldown();
    }

    void ReduceCooldown()
    {
        if (weaponSkill == null) return;
        weaponSkill.ReduceCooldown(cooldownReduceAmount);
    }

    void HandleTagSwitched(GameObject newActive, GameObject newReserve)
    {
       
        if (newActive == this.gameObject)
            OnTagEnter();
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

        while (target != null && Vector3.Distance(transform.position, target.position) > dashStopDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }

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
