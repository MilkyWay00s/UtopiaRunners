using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int CurrentHealth { get; private set; }

    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDeath;

    bool isDead = false;

    void Awake()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        int finalDamage = amount;

        var mods = GetComponents<IDamageModifier>();
        for (int i = 0; i < mods.Length; i++)
        {
            mods[i].ModifyDamage(ref finalDamage);
            if (finalDamage <= 0) break; // 0이면 더 깎을 필요 없음
        }

        // 데미지가 0이 되면HP 변화 없음
        if (finalDamage <= 0) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - finalDamage, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}