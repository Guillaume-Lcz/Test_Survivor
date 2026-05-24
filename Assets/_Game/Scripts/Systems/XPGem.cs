using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class XPGem : MonoBehaviour
{
    [SerializeField] private float xpAmount = 10f;
    [SerializeField] private float pickupRadius = 2f;
    [SerializeField] private float speed = 3.5f;

    private Transform _player;
    private PlayerXP _playerXP;
    private Rigidbody2D _rb;
    private bool _attracted;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.15f;
    }

    private void Start()
    {
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) return;
        _player = playerGO.transform;
        _playerXP = playerGO.GetComponent<PlayerXP>();
    }

    private void Update()
    {
        if (_player == null || _attracted) return;
        if (Vector2.Distance(transform.position, _player.position) <= pickupRadius)
            _attracted = true;
    }

    private void FixedUpdate()
    {
        if (_player == null || !_attracted) return;

        Vector2 next = Vector2.MoveTowards(_rb.position, _player.position, speed * Time.fixedDeltaTime);
        _rb.MovePosition(next);

        if (Vector2.Distance(_rb.position, _player.position) <= 0.2f)
            Collect();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        Collect();
    }

    private void Collect()
    {
        _playerXP?.AddXP(xpAmount);
        Destroy(gameObject);
    }
}
