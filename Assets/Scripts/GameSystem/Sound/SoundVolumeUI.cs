using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; 

public class SoundOptionUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Volume Texts")]
    [SerializeField] private TextMeshProUGUI masterText;
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private TextMeshProUGUI sfxText;

    void Start()
    {
        StartCoroutine(InitSliders());
    }

    IEnumerator InitSliders()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        masterSlider.value = SoundManager.Instance.MasterSoundVolume;
        bgmSlider.value = SoundManager.Instance.BGMSoundVolume;
        sfxSlider.value = SoundManager.Instance.SFXSoundVolume;

        if (masterText != null) UpdateText(masterText, masterSlider.value);
        if (bgmText != null) UpdateText(bgmText, bgmSlider.value);
        if (sfxText != null) UpdateText(sfxText, sfxSlider.value);

        masterSlider.onValueChanged.RemoveAllListeners();
        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);

        bgmSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);

        sfxSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
    }

    private void UpdateText(TextMeshProUGUI textElement, float value)
    {
        if (textElement != null)
        {
            int volumeInt = Mathf.RoundToInt(value * 100f);
            textElement.text = volumeInt.ToString();
        }
    }

    private void OnMasterSliderChanged(float value)
    {
        SoundManager.Instance.MasterSoundVolume = value;
        UpdateText(masterText, value);
    }

    private void OnBGMSliderChanged(float value)
    {
        SoundManager.Instance.BGMSoundVolume = value;
        UpdateText(bgmText, value);
    }

    private void OnSFXSliderChanged(float value)
    {
        SoundManager.Instance.SFXSoundVolume = value;
        UpdateText(sfxText, value);
    }
}