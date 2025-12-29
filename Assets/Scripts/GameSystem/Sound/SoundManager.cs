using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonObject<SoundManager>
{
    //일단 gpt로 대충 생성. 이후 필요 기능에 따라 개선 필요
    [Header("Database")]
    [SerializeField] private SoundSourceList sourceList;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSourcePrefab;
    [SerializeField] private int sfxPoolSize = 10;

    [Header("Volume")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    // 내부 캐시
    private readonly Dictionary<string, SoundSourceList.SoundEntry> _byName = new();
    private readonly List<AudioSource> _sfxPool = new();
    private int _sfxPoolCursor = 0;

    protected override void Awake()
    {
        base.Awake();
        BuildCache();
        SetupSources();
        BuildSfxPool();
        ApplyVolumes();
    }

    private void BuildCache()
    {
        _byName.Clear();

        if (sourceList == null)
        {
            Debug.LogError("SoundManager: sourceList가 할당되지 않았습니다.");
            return;
        }

        foreach (var s in sourceList.sounds)
        {
            if (s == null) continue;
            if (string.IsNullOrEmpty(s.soundName)) continue;

            // 중복 키 방지 (뒤가 덮어쓰게)
            _byName[s.soundName] = s;
        }
    }

    private void SetupSources()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.playOnAwake = false;
        }

        if (sfxSourcePrefab == null)
        {
            // 프리팹을 안 쓰면, 기본 AudioSource를 풀로 생성합니다.
            // (일단은 이 방식이 가장 간단합니다.)
        }
    }

    private void BuildSfxPool()
    {
        _sfxPool.Clear();

        int count = Mathf.Max(1, sfxPoolSize);
        for (int i = 0; i < count; i++)
        {
            AudioSource src;

            if (sfxSourcePrefab != null)
            {
                src = Instantiate(sfxSourcePrefab, transform);
            }
            else
            {
                var go = new GameObject($"SFX_Source_{i}");
                go.transform.SetParent(transform);
                src = go.AddComponent<AudioSource>();
                src.playOnAwake = false;
            }

            // SFX 기본 세팅
            src.loop = false;
            _sfxPool.Add(src);
        }
    }

    public bool Play(string soundName)
    {
        if (!_byName.TryGetValue(soundName, out var entry) || entry.clip == null)
        {
            Debug.LogWarning($"SoundManager: sound not found or clip null: {soundName}");
            return false;
        }

        if (entry.type == SoundType.Bgm)
        {
            PlayBgmInternal(entry);
        }
        else
        {
            PlaySfxInternal(entry);
        }

        return true;
    }

    public void PlayBgm(string soundName)
    {
        if (!_byName.TryGetValue(soundName, out var entry) || entry.clip == null)
        {
            Debug.LogWarning($"SoundManager: BGM not found: {soundName}");
            return;
        }

        PlayBgmInternal(entry);
    }

    public void StopBgm()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    public void StopAllSfx()
    {
        foreach (var s in _sfxPool)
        {
            if (s != null) s.Stop();
        }
    }

    public void StopAll()
    {
        StopBgm();
        StopAllSfx();
    }

    public void SetMasterVolume(float v)
    {
        masterVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    public void SetBgmVolume(float v)
    {
        bgmVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    public void SetSfxVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (bgmSource != null)
            bgmSource.volume = masterVolume * bgmVolume;

        foreach (var s in _sfxPool)
        {
            if (s != null)
                s.volume = masterVolume * sfxVolume;
        }
    }

    private void PlayBgmInternal(SoundSourceList.SoundEntry entry)
    {
        if (bgmSource == null) return;

        // 같은 BGM이면 재시작 안 하게 하고 싶으면 아래 조건을 사용하세요.
        // if (bgmSource.clip == entry.clip && bgmSource.isPlaying) return;

        bgmSource.clip = entry.clip;
        bgmSource.loop = entry.loop;
        bgmSource.volume = masterVolume * bgmVolume * entry.volume;
        bgmSource.Play();
    }

    private void PlaySfxInternal(SoundSourceList.SoundEntry entry)
    {
        var src = GetNextSfxSource();
        if (src == null) return;

        src.clip = entry.clip;
        src.loop = entry.loop;
        src.volume = masterVolume * sfxVolume * entry.volume;

        // loop가 true면 그냥 Play, loop가 false면 OneShot도 가능
        if (entry.loop)
        {
            src.Play();
        }
        else
        {
            // src.PlayOneShot(entry.clip, src.volume);  // 이 방식도 가능
            src.Play();
        }
    }

    private AudioSource GetNextSfxSource()
    {
        if (_sfxPool.Count == 0) return null;

        // 다음 소스를 순환 사용 (가장 단순한 풀)
        var src = _sfxPool[_sfxPoolCursor];
        _sfxPoolCursor = (_sfxPoolCursor + 1) % _sfxPool.Count;

        // 이미 재생 중이면 끊고 새로 재생 (UI 클릭음 같은 용도엔 이게 편합니다)
        if (src.isPlaying && !src.loop)
        {
            src.Stop();
        }

        return src;
    }
}
