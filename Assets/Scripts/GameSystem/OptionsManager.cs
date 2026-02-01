using DG.Tweening;
using InputSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    private GameObject optionWindow;
    private void Update()
    {
        if(InputManager.Instance.GetKeyDown(ActionCode.Option))
        {
            OpenOptions();
        }
    }
    public void OpenOptions()
    {
        if (optionWindow != null) return;
        optionWindow = Instantiate(Resources.Load<GameObject>("UI/UI_Option"));

        if (this.gameObject != null)
        {
            this.gameObject.transform.DOPunchScale(new Vector3(0.35f, 0.7f, 1f) * -0.2f, 0.2f).SetEase(Ease.InBack);
            this.gameObject.GetComponent<Button>().interactable = false;
            optionWindow.GetComponent<OptionWin>().OptionButton = this.gameObject.GetComponent<Button>();
        }

        optionWindow.GetComponent<OptionWin>().Init();
    }
}
