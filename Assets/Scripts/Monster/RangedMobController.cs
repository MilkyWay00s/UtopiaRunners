using UnityEngine;

public class RangedMobController : MonoBehaviour
{
    public MonsterData data;
    Transform player;

    float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("FirePoint").transform;
    }

    void Update()
    {
        if (data == null)
        {
            return;
        }
        timer += Time.deltaTime;

        if (timer >= data.attackInterval)
        {
            timer = 0f;
            Attack();
        }
    }

    void Attack()
    {
        if (data.rangedType == RangedAttackType.Projectile)
            FireProjectile();
        else if (data.rangedType == RangedAttackType.Laser)
            FireLaser();
    }

    void FireProjectile()
    {
        GameObject proj = Instantiate(
            data.projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        proj.GetComponent<MobProjectile>()
            .SetTarget(player.position + data.projectileTargetOffset);
    }

    void FireLaser()
    {
        GameObject laser = Instantiate(
            data.laserPrefab,
            transform.position,
            Quaternion.identity
        );

        MobLaser beam = laser.GetComponent<MobLaser>();
        beam.Init(
            data.laserGrowSpeed,
            data.laserMaxLength,
            data.laserDuration
        );
    }
}
