using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int width = 21;
    public int height = 21;

    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject exitPrefab;

    private int[,] maze;

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
    }

    void Carve(int x, int y)
    {
        int[] dirX = { 0, 0, 3, -3 };
        int[] dirY = { 3, -3, 0, 0 };

        maze[x, y] = 0;
        if (x + 1 < width) maze[x + 1, y] = 0;
        if (y + 1 < height) maze[x, y + 1] = 0;
        if (x + 1 < width && y + 1 < height) maze[x + 1, y + 1] = 0;

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

            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1)
            {
                if (maze[nx, ny] == 1)
                {
                    // ขุดทางกว้าง 2 ช่อง
                    int stepX = dirX[i] / 3;
                    int stepY = dirY[i] / 3;
                    maze[x + stepX, y + stepY] = 0;
                    maze[x + stepX * 2, y + stepY * 2] = 0;
                    if (stepX != 0)
                    {
                        maze[x + stepX, y + stepY + 1] = 0;
                        maze[x + stepX * 2, y + stepY * 2 + 1] = 0;
                    }
                    else
                    {
                        maze[x + stepX + 1, y + stepY] = 0;
                        maze[x + stepX * 2 + 1, y + stepY * 2] = 0;
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