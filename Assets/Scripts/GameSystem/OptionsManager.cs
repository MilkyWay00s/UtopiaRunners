using DG.Tweening;
using InputSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class OptionsManager : MonoBehaviour
{
    private GameObject optionWindow;
    private Button optionButton;

    private void Awake()
    {
        optionButton = GetComponent<Button>();
    }
    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(ActionCode.Option))
        {
            if (optionWindow != null)
            {
                OptionWin optWin = optionWindow.GetComponent<OptionWin>();
                if (optWin != null)
                {
                    optWin.GoBack();
                }
            }
            else
            {
                OpenOptions();
            }
        }
    }
    public void OpenOptions()
    {
        if (optionWindow != null) return;
        SoundManager.Instance.PlaySFX(SFX.SFX1_Click);
        GameObject prefab = Resources.Load<GameObject>("UI/UI_Option");
        Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>();

        if (mainCanvas != null && prefab != null)
        {
            optionWindow = Instantiate(prefab, mainCanvas.transform);

            RectTransform rect = optionWindow.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
            }

            OptionWin script = optionWindow.GetComponent<OptionWin>();
            script.Init();
            script.OnClose = CloseOptions;
        }

        if (optionButton != null)
        {
            optionButton.transform.DOPunchScale(new Vector3(0.35f, 0.7f, 1f) * -0.2f, 0.2f).SetEase(Ease.InBack);
            optionButton.interactable = false;
        }

        optionWindow.GetComponent<OptionWin>().Init();
    }

    public void CloseOptions()
    {
        if (optionButton != null) optionButton.interactable = true;

        optionButton.transform.DOScale(1f, 0.1f);

        optionWindow = null;
    }
}