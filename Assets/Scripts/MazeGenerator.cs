using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int width = 21;
    public int height = 21;
    public float cellSize = 1f;

    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject exitPrefab;

    
    // จำนวนการเปิดผนังเพิ่มเติมเพื่อให้มีทางเลือกหลายทางไปยังทางออก
    public int extraOpenings = 8;
    
    // อัตราส่วนทางตันสูงสุด (เช่น 0.08 = ทางตันไม่เกิน 8% ของช่องทางเดินทั้งหมด)
    public float maxDeadEndRatio = 0.08f;

    private int[,] maze;
    public int[,] MazeData => maze;

    void Start()
    {
        GenerateMaze();
        CreateTopExit();
        DrawMaze();
    }

    void GenerateMaze()
    {
        maze = new int[width, height];

        // เริ่มเป็นกำแพงทั้งหมด
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 1;

        // เพิ่มขอบกั้นด้านบน
        for (int x = 0; x < width; x++)
            maze[x, height - 1] = 1;

        // เพิ่มขอบกั้นด้านขวา
        for (int y = 0; y < height; y++)
            maze[width - 1, y] = 1;

        // เริ่มขุดจาก (1,1)
        Carve(1, 1);

        // สร้างทางเลือกเพิ่มเติมโดยการเปิดผนังสุ่ม
        CreateExtraPaths(extraOpenings);

        // ลดจำนวนทางตันถ้ามากเกินไป
        ReduceDeadEnds(maxDeadEndRatio, extraOpenings * 20);
    }

    void ReduceDeadEnds(float maxRatio, int maxAttempts)
    {
        if (maxRatio <= 0f) return;

        List<Vector2Int> floors = new List<Vector2Int>();
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (maze[x, y] == 0) floors.Add(new Vector2Int(x, y));
            }
        }

        int totalFloors = floors.Count;
        if (totalFloors == 0) return;

        int allowedDeadEnds = Mathf.Max(1, Mathf.RoundToInt(totalFloors * maxRatio));

        int attempts = 0;
        while (attempts < maxAttempts)
        {
            attempts++;

            // หา dead-ends ปัจจุบัน
            List<Vector2Int> deadEnds = new List<Vector2Int>();
            foreach (var p in floors)
            {
                int x = p.x, y = p.y;
                int neighbors = 0;
                if (x + 1 < width && maze[x + 1, y] == 0) neighbors++;
                if (x - 1 >= 0 && maze[x - 1, y] == 0) neighbors++;
                if (y + 1 < height && maze[x, y + 1] == 0) neighbors++;
                if (y - 1 >= 0 && maze[x, y - 1] == 0) neighbors++;
                if (neighbors == 1) deadEnds.Add(p);
            }

            if (deadEnds.Count <= allowedDeadEnds) break; // เพียงพอแล้ว

            // เลือก dead-end แบบสุ่ม
            Vector2Int de = deadEnds[Random.Range(0, deadEnds.Count)];

            // หา wall เพื่อตัดเชื่อม (ไม่ใช่ทิศที่เชื่อมกลับไปยังทางเดินเดิม)
            List<Vector2Int> candidates = new List<Vector2Int>();
            // ตรวจรอบ 4 ทิศ หาเป็นกำแพง
            var neigh = new (int dx, int dy)[] { (1,0), (-1,0), (0,1), (0,-1) };
            foreach (var d in neigh)
            {
                int nx = de.x + d.dx;
                int ny = de.y + d.dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (maze[nx, ny] == 1) candidates.Add(new Vector2Int(nx, ny));
                }
            }

            if (candidates.Count == 0) continue;

            // เลือกกำแพงหนึ่งบล็อกและเปิดเป็นพื้นที่ 3x3 (เพื่อให้เข้ากับทางเดินกว้าง 3)
            Vector2Int wall = candidates[Random.Range(0, candidates.Count)];
            for (int ox = -1; ox <= 1; ox++)
            {
                for (int oy = -1; oy <= 1; oy++)
                {
                    int px = wall.x + ox;
                    int py = wall.y + oy;
                    if (px >= 0 && px < width && py >= 0 && py < height)
                    {
                        if (maze[px, py] == 1)
                        {
                            maze[px, py] = 0;
                            floors.Add(new Vector2Int(px, py));
                        }
                    }
                }
            }
            // loop จะคำนวณ dead-ends ใหม่ในรอบถัดไป
        }
    }

    void CreateExtraPaths(int extra)
    {
        if (extra <= 0) return;

        int made = 0;
        int attempts = 0;
        int maxAttempts = extra * 20;

        while (made < extra && attempts < maxAttempts)
        {
            attempts++;

            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);

            if (maze[x, y] != 1) continue; // ต้องเป็นกำแพงก่อน

            // นับจำนวนช่องพื้นรอบๆ (4 ทิศ)
            int floorNeighbors = 0;
            if (x + 1 < width && maze[x + 1, y] == 0) floorNeighbors++;
            if (x - 1 >= 0 && maze[x - 1, y] == 0) floorNeighbors++;
            if (y + 1 < height && maze[x, y + 1] == 0) floorNeighbors++;
            if (y - 1 >= 0 && maze[x, y - 1] == 0) floorNeighbors++;

            // ถ้ามีพื้นอย่างน้อยสองด้าน แปลว่าการเปิดผนังจะเชื่อมทางเดิน
            if (floorNeighbors >= 2)
            {
                // เปิดพื้นที่เล็กๆ รอบตำแหน่ง เพื่อให้เข้ากับความกว้างทางเดิน (3x3)
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int px = x + dx;
                        int py = y + dy;
                        if (px >= 0 && px < width && py >= 0 && py < height)
                            maze[px, py] = 0;
                    }
                }
                made++;
            }
        }
    }

    void Carve(int x, int y)
    {
        // ใช้ระยะ 5 เพื่อให้กำแพงหนา 2 ช่องและทางเดินกว้าง 3 ช่อง
        int[] dirX = { 0, 0, 5, -5 };
        int[] dirY = { 5, -5, 0, 0 };

        // ทำให้โหนดเป็นบล็อก 3x3 (ทางเดินกว้าง 3)
        for (int dx = 0; dx < 3; dx++)
        {
            for (int dy = 0; dy < 3; dy++)
            {
                int px = x + dx;
                int py = y + dy;
                if (px >= 0 && px < width && py >= 0 && py < height)
                    maze[px, py] = 0;
            }
        }

        List<int> dirs = new List<int> { 0, 1, 2, 3 };
        for (int i = 0; i < dirs.Count; i++)
        {
            int rnd = Random.Range(i, dirs.Count);
            (dirs[i], dirs[rnd]) = (dirs[rnd], dirs[i]);
        }

        foreach (int i in dirs)
        {
            int nx = x + dirX[i];
            int ny = y + dirY[i];

            // ตรวจ bounds โดยพิจารณาจากบล็อก 3x3 และช่องว่างระหว่างโหนด (รวมเป็น 5)
            if (nx > 0 && ny > 0 && nx < width - 3 && ny < height - 3)
            {
                if (maze[nx, ny] == 1)
                {
                    // ขุดทางเชื่อมระยะ 5 ให้เป็นทางเดินกว้าง 3 ช่อง
                    int stepX = dirX[i] / 5; // -1,0,1
                    int stepY = dirY[i] / 5;

                    // ระยะระหว่างโหนดมี 4 ช่อง (x+1 .. x+4), เราต้องเคลียร์ช่องเหล่านั้น
                    for (int dist = 1; dist <= 4; dist++)
                    {
                        if (stepX != 0)
                        {
                            // การเคลื่อนแนวนอน: เคลียร์แถบแนวตั้ง 3 แถว (y .. y+2)
                            for (int dy = 0; dy < 3; dy++)
                            {
                                int px = x + stepX * dist;
                                int py = y + dy;
                                if (px >= 0 && px < width && py >= 0 && py < height)
                                    maze[px, py] = 0;
                            }
                        }
                        else
                        {
                            // การเคลื่อนแนวตั้ง: เคลียร์แถบแนวนอน 3 คอลัมน์ (x .. x+2)
                            for (int dx = 0; dx < 3; dx++)
                            {
                                int px = x + dx;
                                int py = y + stepY * dist;
                                if (px >= 0 && px < width && py >= 0 && py < height)
                                    maze[px, py] = 0;
                            }
                        }
                    }

                    Carve(nx, ny);
                }
            }
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = maze[x, y] == 1 ? wallPrefab : floorPrefab;
                Instantiate(prefab, new Vector2(x, y), Quaternion.identity, transform);
            }
        }
    }

    void CreateTopExit()
    {
        // เลือกตำแหน่งสุ่มที่ด้านบนสุดของแมพ (y = height - 1)
        // ข้ามหน่วยเดียว (ตำแหน่ง 0 และ width - 1 เป็นกำแพง)
        int randomX = Random.Range(1, width - 1);
        
        // ตรวจสอบและหากำแพงอื่นใกล้เคียงเพื่อสร้างทางออกที่เหมาะสม
        if (randomX % 2 == 0)
        {
            randomX--; // ทำให้เป็นเลขคี่เพื่อความเข้ากัน
        }
        
        // สร้างทางออกด้านบน (เลยขอบกำแพง 1 ช่อง)
        int topY = height; // ขึ้นไปด้านบน 1 ช่อง
        maze[randomX, topY - 1] = 0; // ตำแหน่งในแมพ (ช่องก่อนหน้า)
        
        // ขยายทางออกเล็กน้อยเพื่อให้เห็นชัดเจน
        if (randomX + 1 < width - 1)
        {
            maze[randomX + 1, topY - 1] = 0;
        }
        if (randomX - 1 > 0)
        {
            maze[randomX - 1, topY - 1] = 0;
        }
        
        // สร้าง Exit Prefab 3 ตัว ให้เท่ากับจำนวนบล็อกทางออก
        if (exitPrefab != null)
        {
            // Exit ตรงกลาง
            Instantiate(exitPrefab, new Vector2(randomX, topY), Quaternion.identity, transform);
            
            // Exit ด้านซ้าย
            if (randomX - 1 > 0)
            {
                Instantiate(exitPrefab, new Vector2(randomX - 1, topY), Quaternion.identity, transform);
            }
            
            // Exit ด้านขวา
            if (randomX + 1 < width - 1)
            {
                Instantiate(exitPrefab, new Vector2(randomX + 1, topY), Quaternion.identity, transform);
            }
        }
    }
}