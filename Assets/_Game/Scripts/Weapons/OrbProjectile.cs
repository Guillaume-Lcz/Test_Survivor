using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OrbProjectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private float _maxRange;
    private Vector3 _startPosition;

    public void Init(float damage, float speed, float maxRange, Vector2 direction)
    {
        _damage = damage;
        _speed = speed;
        _maxRange = maxRange;
        _startPosition = transform.position;

        var rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
    }

private void Update()
    {
        if (Vector3.Distance(transform.position, _startPosition) >= _maxRange)
            Destroy(gameObject);
    }

private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;

        var damageable = col.GetComponent<IDamageable>();
        if (damageable == null) return;

        damageable.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
