using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SoundData", fileName = "NewSoundData")]
public class SoundData : ScriptableObject
{
    public enum SoundType { BGM,SFX };

    public string key;     
    public AudioClip clip;
    public SoundType type;
    public float volume = 1f;
    public bool loop = false;
}
