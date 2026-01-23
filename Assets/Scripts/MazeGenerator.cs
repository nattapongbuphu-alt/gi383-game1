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

        // เริ่มขุดจาก (1,1)
        Carve(1, 1);
    }

    void Carve(int x, int y)
    {
        int[] dirX = { 0, 0, 2, -2 };
        int[] dirY = { 2, -2, 0, 0 };

        maze[x, y] = 0;

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
                    maze[x + dirX[i] / 2, y + dirY[i] / 2] = 0;
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