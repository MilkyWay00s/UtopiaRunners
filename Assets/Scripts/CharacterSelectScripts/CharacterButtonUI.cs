using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonUI : MonoBehaviour
{
    public int characterIndex;
    public Image icon;
    public Text nameText;

    public void Setup(int index, CharacterSpec spec)
    {
        characterIndex = index;
        icon.sprite = spec.displayImage;
        nameText.text = spec.displayName;
    }
}