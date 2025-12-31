using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text coinText;

    void Update()
    {
        coinText.text = GameManager.Instance.coin.ToString();
    }
}

