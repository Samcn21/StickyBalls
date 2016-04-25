using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObstacleController : MonoBehaviour
{

    public enum GenerationMode
    {
        SingleTile,
        AddSingleTileTop,
        AddSingleTileBottom,
        AddSingleTileRight,
        AddSingleTileLeft,
        AddSymmetryWidth,
        AddSymmetryHeight,
        AddFourSides
    }

    [SerializeField]
    private Transform boardGenerationPoint;
    [SerializeField]
    GameObject obstacleType;
    public List<Vector2> obstacleTiles = new List<Vector2>();
    private List<Vector2> obstacleTilesCompeleteList = new List<Vector2>();
    public GenerationMode[] obstaclesMode;
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
            ObstacleGenerator(boardGenerationPoint.position, obstacleTilesCompeleteList);
        }

        for (int i = 0; i < obstacleTilesCompeleteList.Count; i++)
        {
            //Debug.Log(obstacleTilesCompeleteList[i].x + " - " + obstacleTilesCompeleteList[i].y);
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

        if (obstaclesMode.Length != obstacleTiles.Count)
        {
            Debug.Log("The number of obstacles and the number of mode must be equal!!!");
            return false;
        }
        if (obstacleTiles.GroupBy(n => n).Any(c => c.Count() > 1) || obstacleTiles.Any(par => (int)par.x == 0) || obstacleTiles.Any(par => (int)par.y == 0))
        {
            Debug.Log("There is(are) duplication of obstacles' position or some positions have 0 value");
            return false;
        }
        for (int i = 0; i < obstacleTiles.Count; i++ )
        {
            if (IsAlreadyChosen(obstacleTiles[i]))
            {
                Debug.Log("Tile " + (int)obstacleTiles[i].x + " - " + (int)obstacleTiles[i].y + " is already chosen, please change it to something else");
                return false;
            }
            if ((int)obstacleTiles[i].x <= 0 || (int)obstacleTiles[i].y <= 0)
            {
                Debug.Log("Tile " + (int)obstacleTiles[i].x + " - " + (int)obstacleTiles[i].y + " has a negative value in X or Y, please change it to something positive");
                return false;
            }
            if (!IsOnBoard(obstacleTiles[i]))
            {
                Debug.Log("Tile " + (int)obstacleTiles[i].x + " - " + (int)obstacleTiles[i].y + " has a value out of the board's width or height, please change it to something less than " + gridX + " for X and less than " + gridY + " for Y" );
                return false;                
            }
            if (IsValid(obstacleTiles[i], obstaclesMode[i])){
                Debug.Log("Tile " + (int)obstacleTiles[i].x + " - " + (int)obstacleTiles[i].y + " has a valid value and valid generation mode ");
            }
        }
        return true;
    }
    
    //check if the tile is valid in order to its Generation Mode
    public bool IsValid(Vector2 obstacleTile, GenerationMode obstacleMode) 
    {
        if (obstacleMode == GenerationMode.SingleTile) {
            AddToGenerationList(obstacleTile);
            return true;
        }
        else if (obstacleMode == GenerationMode.AddSingleTileLeft)
        {
            if (IsAddSingleTileLeftValid(obstacleTile))
            {
                AddToGenerationList(obstacleTile);
                AddToGenerationList(new Vector2 (obstacleTile.x - 1 , obstacleTile.y));
                return true;
            }
            return false;
        }
        else if (obstacleMode.ToString().Contains(GenerationMode.AddSymmetryHeight.ToString())) {
            return IsAddFourSidesValid(obstacleTile);
        }
        return false;
    }

    //add the tile to the final generation list
    public void AddToGenerationList(Vector2 obstacleTile)
    {
        obstacleTilesCompeleteList.Add(obstacleTile);
    }

    //check if making a tile on the left side is valid and if it's valid add to the generation list
    public bool IsAddSingleTileLeftValid(Vector2 obstacleTile)
    {
        Vector2 value = new Vector2(obstacleTile.x - 1, obstacleTile.y);
        if (IsAlreadyChosen(value) || !IsOnBoard(value))
        {
            Debug.Log("Tile " + (int)value.x + " - " + (int)value.y + " doesn't have a valid value");
            return false;
        }
        return true;
    }

    //check if making four sides of the tile is valid and if it's valid add to the generation list
    public bool IsAddFourSidesValid(Vector2 obstacleTile) {
        return false;
    }

    //check if a tile is on the board
    public bool IsOnBoard(Vector2 obstacleTile)
    {
        if ((int)obstacleTile.x > gridX || (int)obstacleTile.y > gridY)
        {
            return false;
        }
        return true;
    }
    //check the tile is one of chosen 
    public bool IsAlreadyChosen(Vector2 obstacleTile)
    {
        if (!lockedTilesList.Contains(obstacleTile))
        {
            return false;
        }
        return true;
    }

    //generates the obstacle from obstacleTilesCompeleteList and lock them
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
