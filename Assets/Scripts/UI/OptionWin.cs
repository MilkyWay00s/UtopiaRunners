using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionWin : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Button OptionButton;

    public void Init()
    {
        MasterSlider.minValue = 0.0001f;
        MasterSlider.value = SoundManager.Instance.MasterSoundVolume;
        MasterSlider.onValueChanged.AddListener((float value) => { SoundManager.Instance.MasterSoundVolume = value; });

        BGMSlider.minValue = 0.0001f;
        BGMSlider.value = SoundManager.Instance.BGMSoundVolume;
        BGMSlider.onValueChanged.AddListener((float value) => { SoundManager.Instance.BGMSoundVolume = value; });

        SFXSlider.minValue = 0.0001f;
        SFXSlider.value = SoundManager.Instance.SFXSoundVolume;
        SFXSlider.onValueChanged.AddListener((float value) => { SoundManager.Instance.SFXSoundVolume = value; });
    }

    public void OnEndButton()//메인화면으로
    {
        SoundManager.Instance.PlaySFX(SFX.SFX1_Click);
        OptionButton.interactable = true;
        SceneManager.LoadScene(0);
    }
    public void ExitGame()//게임종료
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnBackButton()
    {
        SoundManager.Instance.PlaySFX(SFX.SFX1_Click);
        OptionButton.interactable = true;
        Destroy(this.gameObject);
    }
}
