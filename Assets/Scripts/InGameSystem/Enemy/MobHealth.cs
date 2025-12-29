using UnityEngine;

public class MobHealth : MonoBehaviour
{
    [Header("몬스터 데이터")]
    public MonsterData data;

    int currentHP;

    void Start()
    {
        if (data == null)
        {
            Debug.LogError("MonsterData가 할당되지 않았습니다.");
            return;
        }

        currentHP = data.maxHp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
