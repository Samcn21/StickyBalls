using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObstacleController : MonoBehaviour
{

    public enum GenerationMode
    {
        OneSide,
        SymmetryWidth,
        SymmetryHeight,
        FourSides
    }

    [SerializeField]
    private Transform boardGenerationPoint;
    [SerializeField]
    GameObject obstacleType;
    public List<Vector2> obstacleTiles = new List<Vector2>();
    public GenerationMode[] obstacleMode;
    public Tile[,] Grid;
    
    private int gridX;
    private int gridY;
    private int numberOfInvoke = 0;


    private List<Vector2> lockedTilesList = new List<Vector2>();
    void Start()
    {
        LockSources();
        GameObject boardObject = GameObject.Find("BOARDGENERATIONPOINT");

        if (boardObject != null && ObstacleTileValidator())
        {
            ObstacleGenerator(boardGenerationPoint.position, obstacleTiles);
        }
    }

    public void LockTiles(List<Vector2> lockedTiles)
    {

        //Debug.Log(numberOfInvoke++);
        lockedTilesList = lockedTiles;

        GridController GridController = GetComponent<GridController>();
        gridX = GridController.GridWidth;
        gridY = GridController.GridHeight;
        Grid = new Tile[gridX, gridY];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                GameObject tileGameObject = GameObject.Find(x + "," + y);
                Tile tile = tileGameObject.GetComponent<Tile>();
                Grid[x, y] = tile;
            }
        }

        //Locks all tiles that are soppused to be locked (Centeral Machine, Sources and Obstacles)
        for (int i = 0; i < lockedTilesList.Count; i++)
        {
            Grid[(int)lockedTilesList[i].x, (int)lockedTilesList[i].y].locked = true;
            //Debug.Log("Tile: " + lockedTilesList[i].x + " - " + lockedTilesList[i].y + " is locked!");
        }
    }

    public void LockSources() { 
        List<Vector2> lockTilesSource = new List<Vector2>();

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                //adds the left bottom source machine to the locked list 
                if (Enumerable.Range(0, 3).Contains(x) && Enumerable.Range(0, 3).Contains(y))
                {
                    lockTilesSource.Add(new Vector2(x, y));
                }
                //adds the left top source machine to the locked list 
                if (Enumerable.Range(0, 3).Contains(x) && Enumerable.Range(gridY - 3, gridY).Contains(y))
                {
                    lockTilesSource.Add(new Vector2(x, y));
                }
                //adds the right bottom source machine to the locked list 
                if (Enumerable.Range(gridX - 3, gridX).Contains(x) && Enumerable.Range(0, 3).Contains(y))
                {
                    lockTilesSource.Add(new Vector2(x, y));
                }
                //adds the right top source machine to the locked list 
                if (Enumerable.Range(gridX - 3, gridX).Contains(x) && Enumerable.Range(gridY - 3, gridY).Contains(y))
                {
                    lockTilesSource.Add(new Vector2(x, y));
                }
            }
        }

        LockTiles(lockTilesSource);
    }

    public bool ObstacleTileValidator() {

        if (obstacleMode.Length != obstacleTiles.Count)
        {
            Debug.Log("The number of obstacles and their Mode must be equal!!!");
            return false;
        }
        if (obstacleTiles.GroupBy(n => n).Any(c => c.Count() > 1) || obstacleTiles.Any(par => (int)par.x == 0) || obstacleTiles.Any(par => (int)par.y == 0))
        {
            Debug.Log("There is(are) duplication of obstacles' position or some positions have 0 value");
            return false;
        }
        for (int i = 0; i < obstacleTiles.Count; i++ )
        {
            if (lockedTilesList.Contains(obstacleTiles[i])) {
                Debug.Log("Tile " + (int)obstacleTiles[i].x + " - " + (int)obstacleTiles[i].y + " is already choosen, please change it to something else");
                return false;
            }
        }

        //TODO:
        //0. first step is to generate 1x1 block!!!
        //1. find x and y in a loop 
        //2. check if we in order to generation mode the obstacle will be in the board

        return true;
    }

    public void ObstacleGenerator(Vector3 originPoint, List<Vector2> obstacleTiles)
    {
        for (int i = 0; i < obstacleTiles.Count; i++)
        {
            GameObject tileGameObject = Instantiate(obstacleType, new Vector3(originPoint.x + obstacleTiles[i].x, originPoint.y , originPoint.z + obstacleTiles[i].y), Quaternion.Euler(90, 0, 0)) as GameObject;
            //Debug.Log("Tile: " + lockedTilesList[i].x + " - " + lockedTilesList[i].y + " is locked!");
        }
        LockTiles(obstacleTiles);
    }
}
