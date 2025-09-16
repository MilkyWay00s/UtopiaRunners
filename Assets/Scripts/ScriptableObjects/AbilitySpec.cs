using UnityEngine;

public enum AbilityTrigger { OnEquip, ActiveOnKey, Passive, Timed }

[CreateAssetMenu(menuName = "Runner/AbilitySpec")]
public class AbilitySpec : ScriptableObject
{
    public string displayName;
    public AbilityTrigger trigger;
    public float duration;
    public float cooldown;
    public string abilityType;
    public float f1, f2, f3;
}