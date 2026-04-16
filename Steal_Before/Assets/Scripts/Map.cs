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
    VerticalEnd,                // Vertical End
    HorizontalEnd,              // Horizontal End
    MiddleL,                    
}

public enum ObstacleType
{
    None,                       // Not an Obstacle
    Player,                     // Player
    Laser,                      // Laser
    Enemy,                      // AI
    Door,                       // Door
    Button,                     // Button
    Goal                        // Goal

}

[System.Serializable]
public struct ObstaclePlacement
{
    public int x;
    public int y;
    public ObstacleType type;
}


[ExecuteAlways]
public class Map : MonoBehaviour
{
    // Public classes
    public int width;
    public int height;
    public float tileSize = 1f;
    public GameObject[] tilePrefabs;
    public GameObject[] obstaclePrefabs;

    [Header("Editor Tools")]
    [Tooltip("Check this box to manually redraw the map after changing a tile in the array below.")]
    public bool refreshMap = false;
    public bool clearMap = false;

    [Header("Saved Map Data")]
    public FloorType[] mapGrid;
    public List<ObstaclePlacement> activeObstacles = new List<ObstaclePlacement>();

    // Private classes
    [SerializeField, HideInInspector] private int lastWidth;
    [SerializeField, HideInInspector] private int lastHeight;
    private Transform boundaryHolder;
    private Transform obstacleHolder;

    void Update()
    {
        if (!Application.isPlaying)
        {
            if (width != lastWidth || height != lastHeight || mapGrid == null || mapGrid.Length != (width * height) || refreshMap)
            {
                // 1. If the map is completely blank, generate from scratch
                if (mapGrid == null || mapGrid.Length == 0)
                {
                    GenerateMapData();
                }
                // 2. If the size changed, do a smart transfer!
                else if (width != lastWidth || height != lastHeight)
                {
                    ResizeMapData(lastWidth, lastHeight);
                }
                // 3. Fallback for data corruption
                else if (mapGrid.Length != (width * height))
                {
                    GenerateMapData();
                }
                else if (refreshMap)
                {
                    ResizeMapData(width, height);
                }

                DrawMap();

                lastWidth = width;
                lastHeight = height;
                refreshMap = false;
            }

            if(clearMap)
            {
                GenerateMapData();
                DrawMap();
                clearMap = false;
            }
        }
    }

    void GenerateMapData()
    {
        // Initialize the 1D array with total area (width * height)
        mapGrid = new FloorType[width * height];

        activeObstacles.Clear();

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
                    ObstaclePlacement emptySlot = new ObstaclePlacement();
                    emptySlot.x = i;
                    emptySlot.y = j;
                    emptySlot.type = ObstacleType.None;

                    activeObstacles.Add(emptySlot);
                }
            }
        }

        Debug.Log("Map Data has been generated");
    }

    void DrawMap()
    {
        ClearOldBoundaries();

        GameObject holderObj = new GameObject("GeneratedBoundaries");
        holderObj.transform.parent = this.transform;
        boundaryHolder = holderObj.transform;

        GameObject obsObj = new GameObject("GeneratedObstacles");
        obsObj.transform.parent = this.transform;
        obstacleHolder = obsObj.transform;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
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

        foreach (ObstaclePlacement obs in activeObstacles)
        {
            // 1. Safety check: Ensure the obstacle is inside the map boundaries!
            if (obs.x >= 0 && obs.x < width && obs.y >= 0 && obs.y < height && obs.type != ObstacleType.None)
            {
                // 2. Safety check: Ensure it's sitting on a valid floor tile
                int floorIndex = obs.x + (obs.y * width);
                if (CanPlaceObstacle(mapGrid[floorIndex]))
                {
                    // Calculate position
                    Vector3 spawnPosition = new Vector3(obs.x * tileSize, obs.y * tileSize, 0.0f);

                    // Spawn the prefab
                    if (((int)obs.type) < obstaclePrefabs.Length && obstaclePrefabs[((int)obs.type)] != null)
                    {
                        GameObject spawnedObs = Instantiate(obstaclePrefabs[((int)obs.type)], spawnPosition, Quaternion.identity, obstacleHolder);
                        spawnedObs.hideFlags = HideFlags.None;
                    }
                }
                else
                {
                    Debug.LogWarning($"Attempted to spawn {obs.type} at {obs.x},{obs.y}, but there is a wall there!");
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

        Transform oldObsHolder = transform.Find("GeneratedObstacles");
        if (oldObsHolder != null)
        {
            DestroyImmediate(oldObsHolder.gameObject);
        }
    }

    void ResizeMapData(int oldWidth, int oldHeight)
    {
        FloorType[] oldGrid = mapGrid;
        mapGrid = new FloorType[width * height];

        List<ObstaclePlacement> newObstacles = new List<ObstaclePlacement>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int newIndex = x + (y * width);

                if ((x == 0 && y == 0) || (x == width - 1 && y == height - 1) || (x == 0 && y == height - 1) || (x == width - 1 && y == 0))
                {
                    mapGrid[newIndex] = FloorType.BaseCornerWall;
                }
                else if ((x == 0 && y <= height - 2) || (x == width - 1 && y <= height - 2))
                {
                    mapGrid[newIndex] = FloorType.VerticalWall;
                }
                else if ((x <= width - 1 && y == 0) || (x <= width - 1 && y == height - 1))
                {
                    mapGrid[newIndex] = FloorType.BaseHorizontalWall;
                }
                else
                {
                    if (x < oldWidth && y < oldHeight && oldGrid != null && oldGrid.Length == (oldWidth * oldHeight))
                    {
                        int oldIndex = x + (y * oldWidth);
                        FloorType oldTile = oldGrid[oldIndex];

                        if (IsBoundaryWall(oldTile))
                        {
                            mapGrid[newIndex] = FloorType.Tile; // Convert old boundary to floor
                        }
                        else
                        {
                            mapGrid[newIndex] = oldTile; // Keep custom painted floors
                        }
                    }
                    else
                    {
                        mapGrid[newIndex] = FloorType.Tile;
                    }

                    if (CanPlaceObstacle(mapGrid[newIndex]))
                    {
                        ObstacleType savedType = ObstacleType.None;
                        foreach (ObstaclePlacement oldObs in activeObstacles)
                        {
                            if (oldObs.x == x && oldObs.y == y)
                            {
                                savedType = oldObs.type;
                                break;
                            }
                        }

                        ObstaclePlacement slot = new ObstaclePlacement();
                        slot.x = x;
                        slot.y = y;
                        slot.type = savedType;

                        newObstacles.Add(slot);
                    }
                }
            }
        }

        activeObstacles = newObstacles;

        Debug.Log($"Map resized! Safely transferred data from {oldWidth}x{oldHeight} to {width}x{height}.");
    }

    bool IsBoundaryWall(FloorType type)
    {
        return type == FloorType.BaseCornerWall ||
               type == FloorType.VerticalWall ||
               type == FloorType.BaseHorizontalWall;
    }

    public bool CanPlaceObstacle(FloorType floorType)
    {
        return floorType == FloorType.Tile;
    }
}