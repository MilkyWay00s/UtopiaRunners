using UnityEngine;

[CreateAssetMenu(menuName = "Runner/WeaponSpec")]
public class WeaponSpec : ScriptableObject
{
    public string displayName;
    public float fireRate;
    public float baseDamage;
    public GameObject projectilePrefab;
    public ModifierSpec[] baseModifiers;
}