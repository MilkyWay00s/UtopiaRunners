using UnityEngine;

public class PlayerHealthTemp : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;

    void Awake()
    {
        currentHp = maxHp;
        Debug.Log($"[PlayerHP] Start HP = {currentHp}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        TakeDamage(1);
    }

    void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(0, currentHp);

        Debug.Log($"[PlayerHP] Hit! HP = {currentHp}");

        if (currentHp <= 0)
        {
            Debug.Log("[PlayerHP] Dead (Temp)");
            gameObject.SetActive(false);
        }
    }
}
