using UnityEngine;

public class FireOrbSpawner : MonoBehaviour
{
    public GameObject fireOrbPrefab;

    // ถ้าต้องการให้เกิดรอบผู้เล่น ให้เซ็ต `player`
    public Transform player; // ถ้าว่างจะใช้ `spawnArea` แบบเดิม

    // ระยะภายใน (ระยะเริ่มต้นจากผู้เล่น) และความยาวหาง (tail length)
    public float innerRadius = 1f; // ระยะเริ่มต้นจากผู้เล่น
    public float tailLength = 2f; // ความยาวหางที่จะสุ่ม (ผลรวม = outerRadius)

    // การันตีอย่างน้อยหนึ่งลูกไฟที่ผู้เล่นเก็บได้
    public bool guaranteeReachable = true; // ถ้า true จะพยายามสปาวน์หนึ่งลูกภายใน `reachableRadius`
    public float reachableRadius = 2f; // ระยะที่ผู้เล่นสามารถเก็บได้
    public int guaranteeAttempts = 10; // จำนวนครั้งสุ่มเมื่อพยายามการันตี

    // จำนวนลูกไฟที่จะเกิดในแต่ละช่วงเวลา และเวลาหมดอายุของลูกไฟ
    public int spawnCount = 1; // จำนวนลูกไฟที่เกิดต่อครั้ง
    public float orbLifetime = 5f; // เวลาที่ลูกไฟจะหายไป (วินาที)
    
    // ระยะขั้นต่ำ (ห้ามเกิดใกล้ผู้เล่นกว่านี้)
    public float minDistanceFromPlayer = 0.5f; // หากระยะจากผู้เล่นน้อยกว่านี้ จะไม่สปาวน์

    public Collider2D spawnArea;
    public float spawnInterval = 3f;

    public LayerMask wallLayer;
    public float checkRadius = 0.3f;
    

    void Start()
    {
        InvokeRepeating(nameof(TrySpawn), 1f, spawnInterval);
    }

    void TrySpawn()
    {
        // ถ้าต้องการการันตีตำแหน่งที่เก็บได้ ให้พยายามหาตำแหน่งภายใน `reachableRadius`
        if (player != null && guaranteeReachable)
        {
            // สุ่มโดยระยะต้องอยู่ระหว่าง `minDistanceFromPlayer` และ `reachableRadius`
            float minC = Mathf.Clamp(minDistanceFromPlayer, 0f, reachableRadius);
            if (minC >= reachableRadius)
            {
                // ถ้าค่าที่ตั้งทำให้เป็นไปไม่ได้ ให้ลด min ชั่วคราว
                minC = reachableRadius * 0.5f;
            }

            for (int attempt = 0; attempt < guaranteeAttempts; attempt++)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float dist = Random.Range(minC, reachableRadius);
                Vector2 candidate = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;
                // ห้ามนอก `spawnArea` — ลูกไฟต้องเกิดเฉพาะภายใน spawnArea (ถ้ามี)
                if (spawnArea != null && !spawnArea.bounds.Contains(candidate))
                    continue;

                if (!Physics2D.OverlapCircle(candidate, checkRadius, wallLayer))
                {
                    if (fireOrbPrefab != null)
                    {
                        GameObject go = Instantiate(fireOrbPrefab, candidate, Quaternion.identity);
                        Destroy(go, orbLifetime);
                    }
                    return; // การันตีสำเร็จ ไม่ต้องสุ่มปกติ
                }
            }
            // ถ้าไม่พบตำแหน่งที่ว่างภายใน attempts ให้ fallback ไปสุ่มปกติ
        }

        Vector2 spawnPos;

        if (player != null)
        {
            // สุ่มตำแหน่งรอบผู้เล่นเป็นวงแหวน (annulus)
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float dist = Random.Range(innerRadius, innerRadius + Mathf.Max(0f, tailLength));
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            spawnPos = (Vector2)player.position + dir * dist;
        }
        else
        {
            // ถ้าไม่มี player ให้ fallback เป็นการสุ่มใน spawnArea
            spawnPos = GetRandomPointInBounds(spawnArea.bounds);
        }

        // สร้าง `spawnCount` ลูก โดยแต่ละลูกสุ่มตำแหน่งตามเงื่อนไขเดิม
        for (int s = 0; s < Mathf.Max(1, spawnCount); s++)
        {
            Vector2 pos;
            if (player != null)
            {
                float lower = Mathf.Max(innerRadius, minDistanceFromPlayer);
                float upper = innerRadius + Mathf.Max(0f, tailLength);
                if (lower >= upper)
                {
                    // หากช่วงไม่สมเหตุสมผล ให้ตั้ง lower เป็นครึ่งหนึ่งของ upper ชั่วคราว
                    lower = upper * 0.5f;
                }

                float angle = Random.Range(0f, Mathf.PI * 2f);
                float dist = Random.Range(lower, upper);
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                pos = (Vector2)player.position + dir * dist;
            }
            else
            {
                pos = GetRandomPointInBounds(spawnArea.bounds);
            }

            // ห้ามนอก `spawnArea` — ลูกไฟต้องเกิดเฉพาะภายใน spawnArea (ถ้ามี)
            if (spawnArea != null && !spawnArea.bounds.Contains(pos))
                continue;

            // ❌ ถ้าตำแหน่งนี้ทับกำแพง → ข้ามไป (ไม่เกิดที่ตำแหน่งนี้)
            if (Physics2D.OverlapCircle(pos, checkRadius, wallLayer))
                continue;

            if (fireOrbPrefab != null)
            {
                GameObject go = Instantiate(fireOrbPrefab, pos, Quaternion.identity);
                Destroy(go, orbLifetime);
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
