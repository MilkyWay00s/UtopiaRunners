using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject OptionsPanel;
    public AudioMixer audioMixer;

    public Slider MasterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider BGMVolumeSlider;

    private const float MinSliderValue = 0.0001f; //Math.Log(0)은 음수 무한대 -> 0으로 초기화 방지

    void Start()
    {
        float masterValue = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgmValue = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 1f);

        MasterVolumeSlider.value = masterValue;
        BGMVolumeSlider.value = bgmValue;
        SFXVolumeSlider.value = sfxValue;

        ApplyVolumeToMixer("MasterVolume", masterValue);
        ApplyVolumeToMixer("BGMVolume", bgmValue);
        ApplyVolumeToMixer("SFXVolume", sfxValue);

        SetMasterVolume(masterValue);
        SetBGMVolume(bgmValue);
        SetSFXVolume(sfxValue);

        MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void OpenOptions() => OptionsPanel.SetActive(true);
    public void CloseOptions() => OptionsPanel.SetActive(false);

    public void SetMasterVolume(float value)
    {
        float v = Mathf.Max(value, MinSliderValue);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", value); 
    }

    public void SetBGMVolume(float value)
    {
        float v = Mathf.Max(value, MinSliderValue);
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("BGMVolume", value); 
    }

    public void SetSFXVolume(float value)
    {
        float v = Mathf.Max(value, MinSliderValue);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(v) * 20f);
        PlayerPrefs.SetFloat("SFXVolume", value); 
    }

    private void ApplyVolumeToMixer(string paramName, float sliderValue)
    {
        float v = Mathf.Max(sliderValue, MinSliderValue);
        audioMixer.SetFloat(paramName, Mathf.Log10(v) * 20f);
    }
}
