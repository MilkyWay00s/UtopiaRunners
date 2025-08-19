using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("���� ����")]
    public float attackPower;
    public float attackSpeed;
    public float critRate;

    [Header("�⺻ ����")]
    public GameObject autoAttackPrefab;

    [Header("��ų")]
    public GameObject skillPrefab;
    public float cooldownTime;
}

