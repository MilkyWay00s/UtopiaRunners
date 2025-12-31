using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject OptionsPanel;
    public GameObject DarkOverlay;
    public AudioMixer audioMixer;
    //public Slider MasterVolumeSlider;
    //public Slider SFXVolumeSlider;
    //public Slider BGMVolumeSlider;

    void Start()
    {
        float masterVol, bgmVol, sfxVol;

        audioMixer.GetFloat("MasterVolume", out masterVol);
        audioMixer.GetFloat("BGMVolume", out bgmVol);
        audioMixer.GetFloat("SFXVolume", out sfxVol);

        //MasterVolumeSlider.value = Mathf.Pow(10, masterVol / 20f);
        //BGMVolumeSlider.value = Mathf.Pow(10, bgmVol / 20f);
        //SFXVolumeSlider.value = Mathf.Pow(10, sfxVol / 20f);

        //MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        //BGMVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        //SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void OpenOptions()
    {
        DarkOverlay.SetActive(true);
        OptionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        DarkOverlay.SetActive(false);
        OptionsPanel.SetActive(false);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20f);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
    }
}
