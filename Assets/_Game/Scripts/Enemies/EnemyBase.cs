using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth = 30f;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float contactDamage = 10f;

    protected float currentHealth;
    protected Rigidbody2D rb;
    protected Transform target;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            target = player.transform;
    }

    protected virtual void FixedUpdate()
    {
        Chase();
    }

    protected virtual void Chase()
    {
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public virtual void TakeDamage(float amount)
    {
        if (amount <= 0f) return;
        currentHealth -= amount;
        if (currentHealth <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats stats = col.gameObject.GetComponent<PlayerStats>();
            stats?.TakeDamage(contactDamage);
        }
    }
}
