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

[ExecuteAlways]
public class Map : MonoBehaviour
{
    // Public classes
    public int width;
    public int height;
    public float tileSize = 1f;
    public GameObject[] tilePrefabs;
    public Transform[] objstaclePos;

    [Header("Editor Tools")]
    [Tooltip("Check this box to manually redraw the map after changing a tile in the array below.")]
    public bool refreshMap = false;

    [Header("Saved Map Data")]
    // 1. THIS IS THE FIX. We changed [,] to [] and made it public.
    // Unity will now permanently save this list to your Scene file!
    public FloorType[] mapGrid;

    // Private classes
    [SerializeField, HideInInspector] private int lastWidth;
    [SerializeField, HideInInspector] private int lastHeight;
    private Transform boundaryHolder;

    void Update()
    {
        if (!Application.isPlaying)
        {
            // 2. We check if the size changed, OR if the array is empty, OR if you clicked the refresh checkbox
            if (width != lastWidth || height != lastHeight || mapGrid == null || mapGrid.Length != (width * height) || refreshMap)
            {
                // We ONLY overwrite the data with the default generation if the physical dimensions changed.
                // This protects your manual edits!
                if (width != lastWidth || height != lastHeight || mapGrid == null || mapGrid.Length != (width * height))
                {
                    GenerateMapData();
                }

                DrawMap();

                lastWidth = width;
                lastHeight = height;
                refreshMap = false; // Uncheck the box automatically
            }
        }
    }

    void GenerateMapData()
    {
        // Initialize the 1D array with total area (width * height)
        mapGrid = new FloorType[width * height];

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                // 3. The Math to flatten X and Y into a single list number
                int index = i + (j * width);

                if ((i == 0 && j == 0) || (i == width - 1 && j == height - 1) || (i == 0 && j == height - 1) || (i == width - 1 && j == 0))
                {
                    mapGrid[index] = FloorType.BaseCornerWall;
                }
                else if ((i == 0 && j <= height - 2) || (i == width - 1 && j <= height - 2))
                {
                    mapGrid[index] = FloorType.VerticalWall;
                }
                else if ((i <= width - 1 && j == 0) || (i <= width - 1 && j == height - 1))
                {
                    mapGrid[index] = FloorType.BaseHorizontalWall;
                }
                else if ((j > 0 && j < height - 1))
                {
                    mapGrid[index] = FloorType.Tile;
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
                // 4. Read from the 1D array using the exact same math formula
                int index = x + (y * width);
                FloorType tileID = mapGrid[index];

                Vector3 spawnPosition = new Vector3(x * tileSize, y * tileSize, 0);

                if (((int)tileID) < tilePrefabs.Length && tilePrefabs[((int)tileID)] != null)
                {
                    GameObject spawnedWall = null;

                    if (tileID == FloorType.BaseHorizontalWall && y == 0)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                    }
                    else if (tileID == FloorType.BaseHorizontalWall && y == height - 1)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                    }
                    else if (tileID == FloorType.VerticalWall && x == 0)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                    }
                    else if (tileID == FloorType.VerticalWall && x == width - 1)
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                    }
                    else if (tileID == FloorType.BaseCornerWall && (x == width - 1 && y == 0))
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                    }
                    else if (tileID == FloorType.BaseCornerWall && (x == 0 && y == height - 1))
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                    }
                    else if (tileID == FloorType.BaseCornerWall && (x == width - 1 && y == height - 1))
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.Euler(new Vector3(180.0f, 180.0f, 0.0f)), boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                    }
                    else if (tileID == FloorType.BaseCornerWall && (x == 0 && y == 0))
                    {
                        spawnedWall = Instantiate(tilePrefabs[((int)tileID)], spawnPosition, Quaternion.identity, boundaryHolder);
                        spawnedWall.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
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
        Transform oldHolder = transform.Find("GeneratedBoundaries");
        if (oldHolder != null)
        {
            DestroyImmediate(oldHolder.gameObject);
        }
    }
}