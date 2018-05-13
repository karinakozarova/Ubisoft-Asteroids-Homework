
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AsteroidsManager : MonoBehaviour {

    protected class PlayableGridCell
    {
        public Bounds cellBounds;
        public bool isOccupied;
    };
    public static AsteroidsManager Instance { get; private set; }

    public float asteroidsSpeed = 3.0f;
    public Vector3 halfExtents = new Vector3(8.8f, 0, 6.6f);
    public float playableGridCellSize = 2.2f;
    PlayableGridCell[,] playableAreaGrid = null;
    public bool drawDebugGrid = false;

    // List of all asteroids objects in the scene
    List<GameObject> asteroids = new List<GameObject>();
    GameObject playerShip = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        //Determine size of the grid, based on the cell size and the playable area size
        int xSize = (int)Mathf.Ceil(2 * halfExtents[0] / playableGridCellSize);
        int zSize = (int)Mathf.Ceil(2 * halfExtents[2] / playableGridCellSize);
        playableAreaGrid = new PlayableGridCell[xSize, zSize];

        //Create an Axis Aligned Bounding Box for each cell in the grid, with the proper center and extents
        Vector3 cellExtents = new Vector3(playableGridCellSize, 0.1f, playableGridCellSize);
        float halfCellSize = playableGridCellSize * 0.5f;

        for (int i = 0; i < xSize; ++i)
        {
            for (int j = 0; j < zSize; ++j)
            {
                Vector3 cellOffset = new Vector3(i, 0, j) * playableGridCellSize + new Vector3(halfCellSize, halfExtents.y, halfCellSize);
                Vector3 cellCenter = -halfExtents + cellOffset;

                playableAreaGrid[i, j] = new PlayableGridCell
                {
                    cellBounds = new Bounds(cellCenter, cellExtents),
                    isOccupied = false
                };
            }
        }

		foreach (GameObject asteroid in asteroids) {
			RandomizeAsteroidTransform(asteroid);
		}
    }

    // Update is called once per frame
    void Update ()
    {
        ClearPlayableAreaGrid();
        foreach (GameObject asteroid in asteroids)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (asteroid.transform.position[i] < -halfExtents[i] || asteroid.transform.position[i] > halfExtents[i])
                {
                    RandomizeAsteroidTransform(asteroid);
                    break;
                }
            }
            MarkOccupiedCells(asteroid);
        }
        MarkOccupiedCells(playerShip);
	}
    void OnDrawGizmos()
    {
        //Used to draw some deubg display, so we can visualize what is on the grid, and which cells are occupied
        if (!drawDebugGrid) return;
        if (playableAreaGrid == null) return;
        for (int i = 0; i < playableAreaGrid.GetLength(0); ++i)
        {
            for (int j = 0; j < playableAreaGrid.GetLength(1); ++j)
            {
                PlayableGridCell cell = playableAreaGrid[i, j];
                Bounds cellBounds = cell.cellBounds;
                Gizmos.color = cell.isOccupied ? Color.red : Color.green;
                Gizmos.DrawCube(cellBounds.center, cellBounds.size - new Vector3(0.05f, 0, 0.05f));
            }
        }
    }

    int GetCellCoordinate(Vector3 position, int worldCoordinateIndex, int gridCoordinateIndex)
    {
        //Convert from world space to Grid space
        //-halfExtents is at 0,0
        //+halfExtents is at grid.Length, grid.Length
        int originCellIndex = playableAreaGrid.GetLength(gridCoordinateIndex) / 2; //(0,0,0) in world space is in the middle of the grid
        int resultingCoordinate = (int)(Mathf.Floor((position[worldCoordinateIndex] / playableGridCellSize))) + originCellIndex;
        resultingCoordinate = Mathf.Clamp(resultingCoordinate, 0, playableAreaGrid.GetLength(gridCoordinateIndex));
        return (int)resultingCoordinate;
    }

    void MarkOccupiedCells(GameObject occupyingObj)
    {
        int i = GetCellCoordinate(occupyingObj.transform.position, 0, 0);
        int j = GetCellCoordinate(occupyingObj.transform.position, 2, 1);
        //The object's pivot is located in grid cell (i,j)
        int minI = Mathf.Max(0, i - 1);
        int maxI = Mathf.Min(i + 1, playableAreaGrid.GetLength(0) - 1);
        int minJ = Mathf.Max(0, j - 1);
        int maxJ = Mathf.Min(j + 1, playableAreaGrid.GetLength(1) - 1);
        //Traverse all 8 cells surrounding the object pivot, and also the cell containing the object
        //Assumption: Every object is strictly smaller than the cell size(an object can't be in more than 4 cells)
        Bounds objectBounds = occupyingObj.GetComponent<MeshRenderer>().bounds;
        Assert.IsTrue(objectBounds.size.x < playableGridCellSize, "Object placed on grid is bigger than grid cell size - this is invalid!");
        Assert.IsTrue(objectBounds.size.z < playableGridCellSize, "Object placed on grid is bigger than grid cell size - this is invalid!");
        for (int iIter = minI; iIter <= maxI; ++iIter)
        {
            for (int jIter = minJ; jIter <= maxJ; ++ jIter)
            {
                Bounds cellBounds = playableAreaGrid[iIter, jIter].cellBounds;
                if (cellBounds.Intersects(objectBounds))
                {
                    playableAreaGrid[iIter, jIter].isOccupied = true;
                }
            }
        }
    }

    void ClearPlayableAreaGrid()
    {
        for (int i = 0; i < playableAreaGrid.GetLength(0); ++i)
        {
            for (int j = 0; j < playableAreaGrid.GetLength(1); ++j)
            {
                playableAreaGrid[i, j].isOccupied = false;
            }
        }
    }

    private void RandomizeAsteroidTransform(GameObject asteroid)
    {
        //Create a list of all guaranteedly unoccupied cells
        List<PlayableGridCell> freeCells = new List<PlayableGridCell>();
        for (int i = 0; i < playableAreaGrid.GetLength(0); ++i)
        {
            for (int j = 0; j < playableAreaGrid.GetLength(1); ++j)
            {
                if (playableAreaGrid[i,j].isOccupied == false)
                {
                    freeCells.Add(playableAreaGrid[i, j]);
                }
            }
        }

        //If there is at least one free cell
        //Teleport the asteroid in its center
        //We can't apply a random offset, because neighbouring cells might not be free
        if (freeCells.Count > 0)
        {
            int chosenCellIndex = Random.Range(0, freeCells.Count);
            PlayableGridCell chosenCell = freeCells[chosenCellIndex];
            Vector3 newPosition = chosenCell.cellBounds.center;
            asteroid.GetComponent<AsteroidController>().RadomizeDirection(newPosition);
        }
    }

    public void RegisterPlayer(GameObject playerObject)
    {
        playerShip = playerObject;
    }
    public void UnregisterPlayer(GameObject gameObject)
    {
        playerShip = null;
    }
    public void RegisterAsteroid(GameObject asteroid)
    {
        asteroids.Add(asteroid);
    }
    public void UnregisterAsteroid(GameObject asteroid)
    {
        asteroids.Remove(asteroid);
    }
}
