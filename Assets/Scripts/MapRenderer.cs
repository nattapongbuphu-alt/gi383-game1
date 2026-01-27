using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public MazeGenerator mazeGenerator;


    public RectTransform mapRoot;
    public GameObject wallIconPrefab;
    public GameObject floorIconPrefab;


    public float mapCellSize = 10f;


    bool generated = false;


    public void GenerateMap()
    {
        if (generated) return;


        int[,] maze = mazeGenerator.MazeData;
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject prefab =
                maze[x, y] == 1 ? wallIconPrefab : floorIconPrefab;


                GameObject icon = Instantiate(prefab, mapRoot);


                RectTransform rt = icon.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(
                x * mapCellSize,
                y * mapCellSize
                );
            }
        }


        generated = true;
    }
}
