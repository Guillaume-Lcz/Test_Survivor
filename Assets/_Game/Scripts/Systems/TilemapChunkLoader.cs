using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapChunkLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase groundTile;

    [Header("Settings")]
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private int loadRadius = 3;

    private readonly HashSet<Vector2Int> _loadedChunks = new();
    private Transform _player;
    private Vector2Int _lastPlayerChunk = Vector2Int.one * int.MaxValue;

private void Start()
    {
        if (tilemap == null) { Debug.LogError("[ChunkLoader] Tilemap ref is NULL!"); return; }
        if (groundTile == null) { Debug.LogError("[ChunkLoader] GroundTile ref is NULL!"); return; }
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO == null) { Debug.LogError("[ChunkLoader] Player not found!"); return; }
        _player = playerGO.transform;
        _lastPlayerChunk = WorldToChunk(_player.position);
        RefreshChunks();
    }

    private void Update()
    {
        Vector2Int currentChunk = WorldToChunk(_player.position);
        if (currentChunk != _lastPlayerChunk)
        {
            _lastPlayerChunk = currentChunk;
            RefreshChunks();
        }
    }

    private void RefreshChunks()
    {
        HashSet<Vector2Int> neededChunks = new();

        for (int x = -loadRadius; x <= loadRadius; x++)
            for (int y = -loadRadius; y <= loadRadius; y++)
                neededChunks.Add(_lastPlayerChunk + new Vector2Int(x, y));

        foreach (var chunk in neededChunks)
            if (_loadedChunks.Add(chunk))
                LoadChunk(chunk);

        var toUnload = new List<Vector2Int>();
        foreach (var chunk in _loadedChunks)
            if (!neededChunks.Contains(chunk))
                toUnload.Add(chunk);

        foreach (var chunk in toUnload)
        {
            UnloadChunk(chunk);
            _loadedChunks.Remove(chunk);
        }
    }

private void LoadChunk(Vector2Int chunk)
    {
        Vector3Int origin = new Vector3Int(chunk.x * chunkSize, chunk.y * chunkSize, 0);
        for (int x = 0; x < chunkSize; x++)
            for (int y = 0; y < chunkSize; y++)
                tilemap.SetTile(origin + new Vector3Int(x, y, 0), groundTile);
        tilemap.RefreshAllTiles();
    }

    private void UnloadChunk(Vector2Int chunk)
    {
        Vector3Int origin = new Vector3Int(chunk.x * chunkSize, chunk.y * chunkSize, 0);
        for (int x = 0; x < chunkSize; x++)
            for (int y = 0; y < chunkSize; y++)
                tilemap.SetTile(origin + new Vector3Int(x, y, 0), null);
    }

    private Vector2Int WorldToChunk(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / chunkSize),
            Mathf.FloorToInt(worldPos.y / chunkSize)
        );
    }
}
