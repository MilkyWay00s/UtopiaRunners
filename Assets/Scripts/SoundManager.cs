using UnityEngine;
using System.Collections.Generic;

public enum BgmType { MainTheme }
public enum SfxType { Jump, Slide } 

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;  
    [SerializeField] private AudioSource sfxSource;  

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> bgmClips;   
    [SerializeField] private List<AudioClip> sfxClips;   

    private Dictionary<BgmType, AudioClip> bgmDict;
    private Dictionary<SfxType, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBGM(BgmType.MainTheme);
    }

    private void Init()
    {
        bgmDict = new Dictionary<BgmType, AudioClip>();
        for (int i = 0; i < bgmClips.Count && i < System.Enum.GetValues(typeof(BgmType)).Length; i++)
        {
            bgmDict[(BgmType)i] = bgmClips[i];
        }

        sfxDict = new Dictionary<SfxType, AudioClip>();
        for (int i = 0; i < sfxClips.Count && i < System.Enum.GetValues(typeof(SfxType)).Length; i++)
        {
            sfxDict[(SfxType)i] = sfxClips[i];
        }
    }

    public void PlayBGM(BgmType type)
    {
        if (bgmDict.TryGetValue(type, out var clip))
        {
            if (bgmSource.clip == clip && bgmSource.isPlaying) return;
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else Debug.LogWarning($"[SoundManager] BGM {type} 없음!");
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(SfxType type)
    {
        if (sfxDict.TryGetValue(type, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else Debug.LogWarning($"[SoundManager] SFX {type} 없음!");
    }
}
