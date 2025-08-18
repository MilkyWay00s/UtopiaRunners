using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpendCoins(int amount)
    {
        if (GameManager.Instance.coins >= amount)
        {
            GameManager.Instance.coins -= amount;
            return true;
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
            return false;
        }
    }

    public void AddCoins(int amount)
    {
        GameManager.Instance.coins += amount;
    }

    public int GetCoins()
    {
        return GameManager.Instance.coins;
    }
}
