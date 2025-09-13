using UnityEngine;
using UnityEngine.UI;
public enum SelectedState { None, Main, Sub}

[CreateAssetMenu(menuName = "Runner/CharacterSpec")]
public class CharacterSpec : ScriptableObject
{
    public string displayName;
    public Sprite displayImage;
    public int characterLevel;
    public AbilitySpec abilities;
    public SelectedState selectedState;
    public GameObject runnerObj;
}