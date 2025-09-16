using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("무기 스탯")]
    public float attackPower;
    public float attackSpeed;
    public float critRate;

    [Header("기본 공격")]
    public GameObject autoAttackPrefab;

    [Header("스킬")]
    public GameObject skillPrefab;
    public float cooldownTime;
}

