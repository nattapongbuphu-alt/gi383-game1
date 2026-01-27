using UnityEngine;
using System.Collections;


public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    // ถ้าต้องการให้เกิดรอบผู้เล่น ให้เซ็ต `player`
    public Transform player; // ถ้าว่างจะใช้ `spawnArea` แบบเดิม

    // ระยะภายในและความยาวหางสำหรับการสุ่มเป็นวงแหวน
    public float innerRadius = 1f;
    public float tailLength = 2f;

    // ระยะขั้นต่ำ (ห้ามเกิดใกล้ผู้เล่นกว่านี้)
    public float minDistanceFromPlayer = 0.5f;

    public Collider2D spawnArea;
    public float spawnInterval = 4f;

    // จำนวนครั้งพยายามหาตำแหน่งที่ว่างก่อนยอมแพ้
    public int spawnAttempts = 10;

    public LayerMask wallLayer;
    public float checkRadius = 0.4f;
    // ถ้าผีไกลจากผู้เล่นเกินค่าต่อไปนี้ จะถูกทำลาย
    public float maxDistanceFromPlayer = 30f;
    
    

    void Start()
    {
        InvokeRepeating(nameof(TrySpawn), 1f, spawnInterval);
    }

    void TrySpawn()
    {
        for (int attempt = 0; attempt < spawnAttempts; attempt++)
        {
            Vector2 spawnPos;

            if (player != null)
            {
                // ปรับ lower bound ให้คำนึงถึง minDistanceFromPlayer
                float lower = Mathf.Max(innerRadius, minDistanceFromPlayer);
                float upper = innerRadius + Mathf.Max(0f, tailLength);
                if (lower >= upper)
                {
                    // หากช่วงไม่สมเหตุสมผล ให้ลด lower ชั่วคราว
                    lower = upper * 0.5f;
                }

                // สุ่มรอบผู้เล่นเป็นวงแหวน (annulus) ระหว่าง lower..upper
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float dist = Random.Range(lower, upper);
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                spawnPos = (Vector2)player.position + dir * dist;
            }
            else
            {
                spawnPos = GetRandomPointInBounds(spawnArea.bounds);
            }

            // ห้ามนอก `spawnArea` — ผีต้องเกิดเฉพาะภายใน spawnArea
            if (spawnArea != null && !spawnArea.bounds.Contains(spawnPos))
                continue;

            // ❌ ถ้าทับกำแพง → ข้าม
            if (Physics2D.OverlapCircle(spawnPos, checkRadius, wallLayer))
                continue;

            if (ghostPrefab != null)
            {
                GameObject go = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
                AutoDespawn ad = go.GetComponent<AutoDespawn>();
                if (ad == null) ad = go.AddComponent<AutoDespawn>();
                ad.player = player;
                ad.maxDistance = maxDistanceFromPlayer;
                break;
            }
        }
    }

    Vector2 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }
    
}
