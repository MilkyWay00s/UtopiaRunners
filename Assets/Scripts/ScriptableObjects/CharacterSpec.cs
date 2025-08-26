using UnityEngine;

[CreateAssetMenu(menuName = "Runner/CharacterSpec")]
public class CharacterSpec : ScriptableObject
{
    public string displayName;
    public float baseMoveSpeed;
    public float baseJumpForce;
    public AbilitySpec[] abilities;
}