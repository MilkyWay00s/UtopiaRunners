using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    public CharacterSpec[] allCharacters;
}
