using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int width = 21;
    public int height = 21;

    public GameObject wallPrefab;
    public GameObject floorPrefab;

    private int[,] maze;

    void Start()
    {
        GenerateMaze();
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
}