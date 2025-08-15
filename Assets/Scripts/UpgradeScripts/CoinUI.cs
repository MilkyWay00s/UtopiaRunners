using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    public TextMeshProUGUI currentCoinText;

    private void Update()
    {
        // GameManager의 코인 값을계속 갱신해서 표시
        currentCoinText.text = "" + GameManager.Instance.coins;
    }
}
