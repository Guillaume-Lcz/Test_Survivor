using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public UnityEvent<float, float> OnHealthChanged; // current, max
    public UnityEvent OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth == 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

private void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }
}
