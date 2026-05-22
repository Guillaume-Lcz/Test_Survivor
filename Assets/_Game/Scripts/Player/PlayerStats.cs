using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invulnerabilityDuration = 1f;

    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private float _invulnerabilityTimer;

    public UnityEvent<float, float> OnHealthChanged; // current, max
    public UnityEvent OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        if (_invulnerabilityTimer > 0f)
            _invulnerabilityTimer -= Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f || _invulnerabilityTimer > 0f) return;
        _invulnerabilityTimer = invulnerabilityDuration;

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
