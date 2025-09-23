using UnityEngine;
using System.Collections.Generic;
using static SoundData;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Database")]
    public SoundDatabase soundDatabase;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    private Dictionary<string, SoundData> soundDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }


    public void Init()
    {
        if (soundDatabase == null)
        {
            Debug.LogError("[SoundManager] SoundDatabase가 할당되지 않았습니다!");
            return;
        }

        soundDict = new Dictionary<string, SoundData>();
        foreach (var sound in soundDatabase.sounds)
        {
            if (!soundDict.ContainsKey(sound.key))
                soundDict.Add(sound.key, sound);
        }
    }

    public void PlayBGM(string key)
    {
        if (soundDict == null)
        {
            Debug.LogWarning("[SoundManager] Init()이 먼저 호출되지 않았습니다!");
            return;
        }

        if (soundDict.TryGetValue(key, out var sound))
        {
            if (sound.type == SoundType.BGM)
            {
                if (bgmSource.clip == sound.clip && bgmSource.isPlaying) return;
                bgmSource.clip = sound.clip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
            else if (sound.type == SoundType.SFX)
            {
                sfxSource.PlayOneShot(sound.clip);
            }
        }
        else Debug.LogWarning($"[SoundManager] {key} 없음!");
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
