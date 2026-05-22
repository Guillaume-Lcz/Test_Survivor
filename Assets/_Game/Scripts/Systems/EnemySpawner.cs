using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Camera mainCamera;

    [Header("Spawn Settings")]
    [SerializeField] private float initialInterval = 3f;
    [SerializeField] private float minimumInterval = 0.5f;
    [SerializeField] private float intervalDecreaseRate = 0.1f;
    [SerializeField] private float intervalDecreaseEvery = 10f;
    [SerializeField] private float spawnMargin = 1.5f;

    private float _currentInterval;
    private float _timer;
    private float _difficultyTimer;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        _currentInterval = initialInterval;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _difficultyTimer += Time.deltaTime;

        if (_difficultyTimer >= intervalDecreaseEvery)
        {
            _difficultyTimer = 0f;
            _currentInterval = Mathf.Max(minimumInterval, _currentInterval - intervalDecreaseRate);
        }

        if (_timer >= _currentInterval)
        {
            _timer = 0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
    }

    private Vector3 GetSpawnPosition()
    {
        float camHeight = mainCamera.orthographicSize + spawnMargin;
        float camWidth = camHeight * mainCamera.aspect + spawnMargin;
        Vector3 camPos = mainCamera.transform.position;

        int side = Random.Range(0, 4);
        return side switch
        {
            0 => new Vector3(camPos.x + Random.Range(-camWidth, camWidth), camPos.y + camHeight, 0f),
            1 => new Vector3(camPos.x + Random.Range(-camWidth, camWidth), camPos.y - camHeight, 0f),
            2 => new Vector3(camPos.x + camWidth, camPos.y + Random.Range(-camHeight, camHeight), 0f),
            _ => new Vector3(camPos.x - camWidth, camPos.y + Random.Range(-camHeight, camHeight), 0f),
        };
    }

    public void SpawnBatch(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
            Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
    }
}
