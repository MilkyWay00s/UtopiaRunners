using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionWin : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Button OptionButton;

    public void Init()
    {
        BGMSlider.minValue = 0.0001f;
        BGMSlider.value = SoundManager.Instance.BGMSoundVolume;
        BGMSlider.onValueChanged.AddListener((float value) => { SoundManager.Instance.BGMSoundVolume = value; });

        SFXSlider.minValue = 0.0001f;
        SFXSlider.value = SoundManager.Instance.SFXSoundVolume;
        SFXSlider.onValueChanged.AddListener((float value) => { SoundManager.Instance.SFXSoundVolume = value; });
    }

    public void OnEndButton()//메인화면으로
    {
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
        OptionButton.interactable = true;
        Destroy(this.gameObject);
    }
}
