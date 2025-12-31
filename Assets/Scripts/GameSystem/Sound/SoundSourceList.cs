using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    None,
    Bgm,
    UI,
    Enemy,
    Voice
}
[CreateAssetMenu(fileName = "SoundSourceList", menuName = "Audio/SoundSourceList")]
public class SoundSourceList : ScriptableObject
{
    [System.Serializable]
    public class SoundEntry
    {
        public string soundName;
        public SoundType type;
        public AudioClip clip;
        public float volume = 1f;
        public bool loop;
    }

    public List<SoundEntry> sounds = new List<SoundEntry>();
}
