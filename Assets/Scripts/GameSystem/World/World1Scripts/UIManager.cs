using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        coinText.text = GameManager.Instance.coins.ToString();
    }

    public void onWorldMapClicked()
    {
        SceneManager.LoadScene("WorldMap");
    }
}
