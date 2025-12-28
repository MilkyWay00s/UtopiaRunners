using UnityEngine;

public enum MonsterAttackType
{
    Melee,
    Ranged
}

public enum RangedAttackType
{
    Projectile,
    Laser
}

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("기본 정보")]
    public string monsterName;

    [Header("스탯")]
    public int maxHp;
    public int defense;

    [Header("공격 타입")]
    public MonsterAttackType attackType;

    [Header("원거리 세부 타입")]
    public RangedAttackType rangedType;

    [Header("공격 속도")]
    public float attackInterval = 1.5f;

    [Header("투사체 타입 공격")]
    public GameObject projectilePrefab;
    public Vector3 projectileTargetOffset;

    [Header("레이저 타입 공격")]
    public GameObject laserPrefab;
    public float laserGrowSpeed;
    public float laserMaxLength;
    public float laserDuration;
}