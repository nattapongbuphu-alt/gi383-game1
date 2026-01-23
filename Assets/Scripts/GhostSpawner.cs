using UnityEngine;
using System.Collections;


public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Collider2D spawnArea;
    public float spawnInterval = 4f;

    public LayerMask wallLayer;
    public float checkRadius = 0.4f;

    void Start()
    {
        InvokeRepeating(nameof(TrySpawn), 1f, spawnInterval);
    }

    void TrySpawn()
    {
        Vector2 spawnPos = GetRandomPointInBounds(spawnArea.bounds);

        // ❌ ถ้าทับกำแพง → ไม่เกิด
        if (Physics2D.OverlapCircle(spawnPos, checkRadius, wallLayer))
            return;

        Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }
    
}
