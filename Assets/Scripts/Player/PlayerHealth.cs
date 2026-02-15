using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int CurrentHealth { get; private set; }

    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDeath;

    bool isDead = false;

    
    [Header("Debug")]
    public bool logDamage = true;

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
        if (logDamage)
            Debug.Log($"[HP] Incoming={amount}, mods={mods.Length} ({gameObject.name})");

        for (int i = 0; i < mods.Length; i++)
        {
            int before = finalDamage;
            mods[i].ModifyDamage(ref finalDamage);

            if (logDamage && before != finalDamage)
                Debug.Log($"[HP] Modifier {mods[i].GetType().Name}: {before} -> {finalDamage}");

            if (finalDamage <= 0) break;
        }

        if (finalDamage <= 0)
        {
            if (logDamage)
                Debug.Log($"[HP] Damage blocked! (cur={CurrentHealth}/{maxHealth}) ({gameObject.name})");
            return;
        }

        int beforeHp = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth - finalDamage, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (logDamage)
            Debug.Log($"[HP] Took {finalDamage}. {beforeHp} -> {CurrentHealth} / {maxHealth} ({gameObject.name})");

        if (CurrentHealth <= 0)
        {
            isDead = true;
            if (logDamage) Debug.Log($"[HP] DEAD ({gameObject.name})");
            OnDeath?.Invoke();
        }
    }

    // Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!logDamage) return;

        if (other.CompareTag("Boss") || other.CompareTag("Enemy"))
        {
            Debug.Log($"[HP] Trigger hit by {other.name} tag={other.tag} (self={gameObject.name})");
            TakeDamage(10);
        }
    }

    // Collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!logDamage) return;

        if (other.gameObject.CompareTag("Boss") || other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"[HP] Collision hit by {other.gameObject.name} tag={other.gameObject.tag} (self={gameObject.name})");
            TakeDamage(10);
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (logDamage)
            Debug.Log($"[HP] Heal {amount}. cur={CurrentHealth}/{maxHealth} ({gameObject.name})");
    }

    public void ApplyMaxHealthBonusAndRefill(int bonus)
    {
        maxHealth += bonus;
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (logDamage)
            Debug.Log($"[HP] ApplyMaxHealthBonus {bonus}. cur={CurrentHealth}/{maxHealth} ({gameObject.name})");
    }
}
