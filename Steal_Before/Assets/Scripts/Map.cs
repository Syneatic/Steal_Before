using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorType 
{
    EdgeWall,
    VerticalEdgeWall,
    HorizontalEdgeWall,
    Space,
    Obstacle
}

public class Map : MonoBehaviour
{
    // Public classes
    public int width;
    public int height;
    public float tileSize = 1f;
    public GameObject[] tilePrefabs;
    public Transform[] objstaclePos;

    // Private classes
    FloorType [,] mapGrid;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMapData();
        DrawMap();
    }

    void GenerateMapData()
    {
        mapGrid = new FloorType[width, height];

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if((i == 0 && j == 0) || (i == width - 1 && j == height - 1) || (i == 0 && j == height - 1) || (i == width -1 && j == 0))
                {
                    mapGrid[i, j] = FloorType.EdgeWall; // Wall ID
                }
                else if ((i == 0 && j <= height - 2) || (i == width - 1 && j <= height - 2))
                {
                    mapGrid[i, j] = FloorType.VerticalEdgeWall; // Wall ID
                }
                else if ((i <= width - 1 && j == 0) || (i <= width - 1 && j == height - 1))
                {
                    mapGrid[i, j] = FloorType.HorizontalEdgeWall; ; // Wall ID
                }
                else if((j > 0 && j < height - 1))
                {
                    mapGrid[i, j] = FloorType.Space; // Floor ID
                }
            }
        }
        Debug.Log("mapArray has a totoal of " + mapGrid.Length + " stored data");
    }

    void DrawMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                FloorType tileID = mapGrid[x, y];

                // Skip ID 0 if we consider it "Empty space"
                //if (tileID == 0) continue;

                Vector3 spawnPosition = new Vector3(x * tileSize, y * tileSize, 0);
                // Check if we have a prefab assigned for this ID to avoid errors
                if (((int)tileID) < tilePrefabs.Length && tilePrefabs[((int)tileID)] != null)
                {
                    // Calculate the world position based on the array coordinates
                    // We use (x, 0, y) for a 3D ground map, or (x, y, 0) for a 2D game.
                    if (tileID == FloorType.HorizontalEdgeWall && y == 0)
                    {
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, this.transform);
                    }
                    else if(tileID == FloorType.HorizontalEdgeWall && y == height - 1)
                    {
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)), this.transform);
                    }
                    else if(tileID == FloorType.VerticalEdgeWall && x == 0)
                    {
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, this.transform);
                    }
                    else if(tileID == FloorType.VerticalEdgeWall && x == width -1)
                    {
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)), this.transform);
                    }
                    else if(tileID == FloorType.EdgeWall && (x == width - 1 && y == 0))
                    {
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)), this.transform);
                    }
                    else if(tileID == FloorType.EdgeWall && (x == 0 && y == height - 1))
                    {
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)), this.transform);
                    }
                    else if(tileID == FloorType.EdgeWall && (x == width - 1 && y == height - 1))
                    {
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 180.0f, 0.0f)), this.transform);
                    }
                    else
                    {
                        // Instantiate the prefab at the calculated position
                        Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, this.transform);
                    }

                }
                else
                {
                    Debug.LogWarning($"No prefab assigned for tile ID: {tileID}");
                }
            }
        }
    }
}