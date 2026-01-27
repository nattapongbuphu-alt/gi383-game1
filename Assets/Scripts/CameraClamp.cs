using UnityEngine;

// จำกัดตำแหน่งกล้องไม่ให้เกินขอบแมพ
// - ถ้าใช้ `useMazeGenerator` และตั้ง `mazeGenerator` จะคำนวนขอบจาก width/height ของ MazeGenerator
// - ไม่เช่นนั้น ให้ตั้ง `spawnArea` (Collider2D) ใน Inspector
public class CameraClamp : MonoBehaviour
{
    public Camera targetCamera;
    public Collider2D spawnArea; // กำหนดขอบเขตแมพ
    public MazeGenerator mazeGenerator; // ถ้าต้องการคำนวนจาก MazeGenerator
    public bool useMazeGenerator = false;

    // ถ้าต้องการให้กล้องตามผู้เล่นเสมอ ให้กำหนด `player` และเปิด `followPlayer`
    public Transform player;
    public bool followPlayer = true;
    // ถ้าอยากให้กล้องเกลี่ยแบบนุ่ม ให้ตั้งค่านี้ (>0), 0 = ติดตามทันที
    public float followSmooth = 10f;

    // padding จะเลื่อนขอบเข้าหาภายใน (หน่วงไม่ให้ชนขอบจริง)
    public Vector2 padding = Vector2.zero;

    void Start()
    {
        if (targetCamera == null) targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector2 min, max;

        if (useMazeGenerator && mazeGenerator != null)
        {
            min = new Vector2(0f, 0f);
            // ปรับให้ใช้ขอบสุดของบล็อก (width-1,height-1) เพื่อไม่ให้กล้องเลยขอบบน/ขวา
            max = new Vector2(mazeGenerator.width - 1f, mazeGenerator.height - 1f);
        }
        else if (spawnArea != null)
        {
            Bounds b = spawnArea.bounds;
            min = b.min;
            max = b.max;
        }
        else
        {
            // ไม่มีขอบเขตให้ใช้ ไม่ต้องจำกัด
            return;
        }

        float halfHeight = targetCamera.orthographicSize;
        float halfWidth = halfHeight * targetCamera.aspect;

        Vector3 camPos = targetCamera.transform.position;

        // ถ้าต้องการให้กล้องตามผู้เล่น ให้ตั้งตำแหน่งเป้าหมายเป็นตำแหน่งผู้เล่น (พร้อมเกลี่ยถ้าตั้งค่า)
        if (followPlayer && player != null)
        {
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, camPos.z);
            if (followSmooth > 0f)
                camPos = Vector3.Lerp(camPos, targetPos, Mathf.Clamp01(followSmooth * Time.deltaTime));
            else
                camPos = targetPos;
        }

        float minX = min.x + halfWidth + padding.x;
        float maxX = max.x - halfWidth - padding.x;
        float minY = min.y + halfHeight + padding.y;
        float maxY = max.y - halfHeight - padding.y;

        // ถ้าพื้นที่เล็กกว่าขนาดกล้อง ให้กึ่งกลางไปที่กลางพื้นที่
        if (minX > maxX)
            camPos.x = (min.x + max.x) * 0.5f;
        else
            camPos.x = Mathf.Clamp(camPos.x, minX, maxX);

        if (minY > maxY)
            camPos.y = (min.y + max.y) * 0.5f;
        else
            camPos.y = Mathf.Clamp(camPos.y, minY, maxY);

        targetCamera.transform.position = new Vector3(camPos.x, camPos.y, targetCamera.transform.position.z);
    }
}
