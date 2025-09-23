using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SoundDatabase", fileName = "SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    public List<SoundData> sounds;
}