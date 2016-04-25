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
        int middleX = Mathf.FloorToInt(gridX / 2);
        int middleY = Mathf.FloorToInt(gridY / 2);

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
            if ((int)obstacleTiles[i].x == middleX || (int)obstacleTiles[i].y == middleY)
            {

                Debug.Log("Tile " + (int)obstacleTiles[i].x + " - " + (int)obstacleTiles[i].y + " is in the center, please change the value of X or Y or both");
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
            if (IsAddSingleTileAroundValid(obstacleTile, -1 , 0))
            {
                AddToGenerationList(obstacleTile);
                AddToGenerationList(new Vector2 (obstacleTile.x - 1 , obstacleTile.y));
                return true;
            }
            return false;
        }
        else if (obstacleMode == GenerationMode.AddSingleTileRight)
        {
            if (IsAddSingleTileAroundValid(obstacleTile , +1 , 0))
            {
                AddToGenerationList(obstacleTile);
                AddToGenerationList(new Vector2(obstacleTile.x + 1, obstacleTile.y));
                return true;
            }
            return false;
        }
        else if (obstacleMode == GenerationMode.AddSingleTileTop)
        {
            if (IsAddSingleTileAroundValid(obstacleTile, 0 , +1))
            {
                AddToGenerationList(obstacleTile);
                AddToGenerationList(new Vector2(obstacleTile.x, obstacleTile.y + 1 ));
                return true;
            }
            return false;
        }
        else if (obstacleMode == GenerationMode.AddSingleTileBottom)
        {
            if (IsAddSingleTileAroundValid(obstacleTile, 0, -1))
            {
                AddToGenerationList(obstacleTile);
                AddToGenerationList(new Vector2(obstacleTile.x, obstacleTile.y - 1));
                return true;
            }
            return false;
        }
        else if (obstacleMode == GenerationMode.AddSymmetryWidth)
        {
            if (IsAddSymmetryValid(obstacleTile, true))
            {
                AddToGenerationList(obstacleTile);
                AddToGenerationList(GetSymmetryValue(obstacleTile, true));
                return true;
            }
            return false;
        }
        else if (obstacleMode == GenerationMode.AddSymmetryHeight)
        {
            if (IsAddSymmetryValid(obstacleTile, false))
            {
                AddToGenerationList(obstacleTile);
                AddToGenerationList(GetSymmetryValue(obstacleTile, false));
                return true;
            }
            return false;
        }
        else if (obstacleMode == GenerationMode.AddFourSides)
        {
            if (IsAddFourSidesValid(obstacleTile))
            {
                for (int i = 0; i < GetFourSidesValue(obstacleTile).Length; i++)
                {
                    AddToGenerationList(GetFourSidesValue(obstacleTile)[i]);
                    //Debug.Log(GetFourSidesValue(obstacleTile)[i]);
                }
                return true;
            }
            return false;
        }
        return false;
    }

    //add the tile to the final generation list
    public void AddToGenerationList(Vector2 obstacleTile)
    {
        obstacleTilesCompeleteList.Add(obstacleTile);
    }

    //check if making a tile on the left,right,top and bottom side is valid
    public bool IsAddSymmetryValid(Vector2 obstacleTile, bool XY)
    {
        //XY = true means check width
        if (XY)
        {
            if (Mathf.Abs(gridX - 1 - obstacleTile.x) == obstacleTile.x || IsAlreadyChosen(new Vector2(Mathf.Abs(gridX - 1 - obstacleTile.x), obstacleTile.y)))
            {
                Debug.Log("Tile " + (int)obstacleTile.x + " - " + (int)obstacleTile.y + " has a symmetry value on itself or already chosen, please change the X value");
                return false;
            }
            else
            {
                return true;
            }
        }
        //XY = false means check height
        else 
        {
            if (Mathf.Abs(gridY - 1 - obstacleTile.y) == obstacleTile.y || IsAlreadyChosen(new Vector2(obstacleTile.x, Mathf.Abs(gridY - 1 - obstacleTile.y))))
            {
                Debug.Log("Tile " + (int)obstacleTile.x + " - " + (int)obstacleTile.y + " has a symmetry value on itself or already chosen, please change the Y value");
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public Vector2 GetSymmetryValue(Vector2 obstacleTile, bool XY) 
    {
        //XY = true means check width
        if (XY)
        {
            return new Vector2(Mathf.Abs(gridX - 1 - obstacleTile.x ), obstacleTile.y);
        }
        //XY = false means check height
        else
        {
            return new Vector2(obstacleTile.x, Mathf.Abs(gridY - 1 - obstacleTile.y));
        }
    }

    //check if making a tile on the left,right,top and bottom side is valid
    public bool IsAddSingleTileAroundValid(Vector2 obstacleTile, int vlaueX, int valueY)
    {
        Vector2 value = new Vector2(obstacleTile.x + vlaueX, obstacleTile.y + valueY);
        if (IsAlreadyChosen(value) || !IsOnBoard(value))
        {
            Debug.Log("Tile " + (int)value.x + " - " + (int)value.y + " doesn't have a valid value");
            return false;
        }
        return true;
    }

    //check if making four sides of the tile is valid and if it's valid add to the generation list
    public bool IsAddFourSidesValid(Vector2 obstacleTile) {
        if (IsAddSymmetryValid(obstacleTile, false) && IsAddSymmetryValid(obstacleTile, true))
        {
            return true;
        }
        Debug.Log("Tile " + (int)obstacleTile.x + " - " + (int)obstacleTile.y + " cannot have 4 sides, please change the X or Y or both X and Y value");
        return false;
    }

    //check if making four sides of the tile is valid and if it's valid add to the generation list
    public Vector2[] GetFourSidesValue(Vector2 obstacleTile) 
    {
        Vector2[] fourSidesTiles = new Vector2[4];
        fourSidesTiles[0] = obstacleTile;
        fourSidesTiles[1] = GetSymmetryValue(obstacleTile, false);
        fourSidesTiles[2] = GetSymmetryValue(obstacleTile, true);
        fourSidesTiles[3] = GetSymmetryValue(GetSymmetryValue(obstacleTile, false), true);
        return fourSidesTiles;
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
