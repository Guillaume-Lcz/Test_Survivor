using System.Collections;
using UnityEngine;

public class OrbShooter : MonoBehaviour, IWeapon
{
    [Header("Settings")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float baseCooldown = 1.5f;
    [SerializeField] private float projectileSpeed = 4f;
    [SerializeField] private float maxRange = 10f;
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private int projectileCount = 1;

    private float _fireRateBonus = 0f;
    private float EffectiveCooldown => baseCooldown / (1f + _fireRateBonus);

    public float Damage { get => damage; set => damage = value; }
    public float FireRateBonus { get => _fireRateBonus; set => _fireRateBonus = value; }
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float Range { get => maxRange; set => maxRange = value; }
    public int ProjectileCount { get => projectileCount; set => projectileCount = value; }

    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Camera mainCamera;

    private float _timer;
    private Vector2 _lastDirection;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= EffectiveCooldown)
        {
            _timer = 0f;
            Activate();
        }
    }

    public void Activate()
    {
        Transform nearest = FindNearestVisibleEnemy();
        Vector2 direction = nearest != null
            ? (Vector2)(nearest.position - transform.position).normalized
            : _lastDirection;

        if (direction == Vector2.zero) return;
        _lastDirection = direction;

        StartCoroutine(FireBurst(direction));
    }

    private IEnumerator FireBurst(Vector2 direction)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 spawnPos = transform.position + (Vector3)(direction * 0.2f);
            GameObject orb = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            orb.GetComponent<OrbProjectile>().Init(damage, projectileSpeed, maxRange, direction);
            if (i < projectileCount - 1)
                yield return new WaitForSeconds(0.12f);
        }
    }

    private Transform FindNearestVisibleEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Vector3 viewport = mainCamera.WorldToViewportPoint(hit.transform.position);
            bool inView = viewport.x >= 0f && viewport.x <= 1f && viewport.y >= 0f && viewport.y <= 1f;
            if (!inView) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
}
