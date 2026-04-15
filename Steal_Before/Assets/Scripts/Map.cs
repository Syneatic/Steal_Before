using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorType
{
    BaseCornerWall,             // Base Corner Wall     #DON'T MOVE
    VerticalWall,               // Vertical Wall        #DON'T MOVE
    BaseHorizontalWall,         // Base Horizontal Wall #DON'T MOVE
    Tile,                       // Tile                 #DON'T MOVE
    TopCornerWall,              // Top Corner Wall
    VerticleWallMiddle,         // Verticle Wall (Middle)
    MiddleWalls,                // Middle Walls 
    PlusBlock,                  // Plus Block
    TBlockHoriontal,            // T Block Hori
    TBlockVerti,                // T Block Verti
    TopHorizontalWall,          // Top HWall
    Obstacle
}

// 1. This tag forces the script to run in the Editor without pressing Play
[ExecuteAlways]
public class Map : MonoBehaviour
{
    // Public classes
    public int width;
    public int height;
    public float tileSize = 1f;
    public GameObject[] tilePrefabs;
    public Transform[] objstaclePos;

    // Private classes
    FloorType[,] mapGrid;

    // We use these to check if the user changed the size in the Inspector
    private int lastWidth;
    private int lastHeight;
    private Transform boundaryHolder;

    // 2. We use Update to detect changes in Edit Mode
    void Update()
    {
        // Only run this logic if we are NOT in Play Mode
        if (!Application.isPlaying)
        {
            // Only redraw the map IF the width or height was actually changed.
            // (Without this check, Unity would try to redraw the map 60 times a second and crash!)
            if (width != lastWidth || height != lastHeight)
            {
                GenerateMapData();
                DrawMap();

                // Save the new dimensions so we don't redraw again until they change
                lastWidth = width;
                lastHeight = height;
            }
        }
    }

    void GenerateMapData()
    {
        mapGrid = new FloorType[width, height];

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if ((i == 0 && j == 0) || (i == width - 1 && j == height - 1) || (i == 0 && j == height - 1) || (i == width - 1 && j == 0))
                {
                    mapGrid[i, j] = FloorType.BaseCornerWall;
                }
                else if ((i == 0 && j <= height - 2) || (i == width - 1 && j <= height - 2))
                {
                    mapGrid[i, j] = FloorType.VerticalWall;
                }
                else if ((i <= width - 1 && j == 0) || (i <= width - 1 && j == height - 1))
                {
                    mapGrid[i, j] = FloorType.BaseHorizontalWall;
                }
                else if ((j > 0 && j < height - 1))
                {
                    mapGrid[i, j] = FloorType.Tile;
                }
            }
        }
        Debug.Log("mapArray has a total of " + mapGrid.Length + " stored data");
    }

    void DrawMap()
    {
        ClearOldBoundaries();

        GameObject holderObj = new GameObject("GeneratedBoundaries");
        holderObj.transform.parent = this.transform;
        boundaryHolder = holderObj.transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                FloorType tileID = mapGrid[x, y];
                Vector3 spawnPosition = new Vector3(x * tileSize, y * tileSize, 0);

                if (((int)tileID) < tilePrefabs.Length && tilePrefabs[((int)tileID)] != null)
                {
                    // 1. Create a temporary variable to hold the wall we are about to spawn
                    GameObject spawnedWall = null;

                    if (tileID == FloorType.BaseHorizontalWall && y == 0)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable;
                    }
                    else if (tileID == FloorType.BaseHorizontalWall && y == height - 1)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable;
                    }
                    else if (tileID == FloorType.VerticalWall && x == 0)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable;
                    }
                    else if (tileID == FloorType.VerticalWall && x == width - 1)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable;
                    }
                    else if (tileID == FloorType.BaseCornerWall && (x == width - 1 && y == 0))
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable;
                    }
                    else if (tileID == FloorType.BaseCornerWall && (x == 0 && y == height - 1))
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable;
                    }
                    else if (tileID == FloorType.BaseCornerWall && (x == width - 1 && y == height - 1))
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 180.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable;
                    }
                    else
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.None;
                    }   
                }
            }
        }
    }

    void ClearOldBoundaries()
    {
        // Find the specific folder holding our generated walls and destroy it
        Transform oldHolder = transform.Find("GeneratedBoundaries");
        if (oldHolder != null)
        {
            DestroyImmediate(oldHolder.gameObject);
        }
    }
}