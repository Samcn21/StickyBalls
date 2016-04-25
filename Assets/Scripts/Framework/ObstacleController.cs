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
    GameObject obstacleType;
    public List<Vector2> obstaclePos = new List<Vector2>();
    public GenerationMode[] obstacleMode;
    public Tile[,] Grid;
    
    void Start()
    {


        if (obstacleMode.Length == obstaclePos.Count)
        {
            //TODO:
            //0. first step is to generate 1x1 block!!!
            //1. find x and y in a loop 
            //2. check if we in order to generation mode the obstacle will be in the board
            //3. find the generated tile in the board
            //4. copy found tile's position to initialized obstacle
            //4.5 generating obstacles around the central machine and sources must be impossible
            //5. check the mode and generate obstacle to related position
            //6. lock all the obstacle positions
        }

    }
    public void Initialize(List<Vector2> lockedPos)
    {
        List<Vector2> lockedPosObstacle = new List<Vector2>();
        lockedPosObstacle = lockedPos;

        GridController GridController = GetComponent<GridController>();
        int gridX = GridController.GridWidth;
        int gridY = GridController.GridHeight;

        Grid = new Tile[GridController.GridWidth, GridController.GridHeight];

        //find all tiles and their grid and also adds all source machines' tiles (3x3) to the list
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                //adds the left bottom source machine to the locked list 
                if (Enumerable.Range(0, 3).Contains(x) && Enumerable.Range(0, 3).Contains(y))
                {
                    lockedPosObstacle.Add(new Vector2(x,y));
                }
                //adds the left top source machine to the locked list 
                if (Enumerable.Range(0, 3).Contains(x) && Enumerable.Range(gridY - 3, gridY).Contains(y))
                {
                    lockedPosObstacle.Add(new Vector2(x, y));
                }
                //adds the right bottom source machine to the locked list 
                if (Enumerable.Range(gridX - 3, gridX).Contains(x) && Enumerable.Range(0, 3).Contains(y))
                {
                    lockedPosObstacle.Add(new Vector2(x, y));
                }
                //adds the right top source machine to the locked list 
                if (Enumerable.Range(gridX - 3, gridX).Contains(x) && Enumerable.Range(gridY - 3, gridY).Contains(y))
                {
                    lockedPosObstacle.Add(new Vector2(x, y));
                }

                GameObject tileGameObject = GameObject.Find(x + "," + y);
                Tile tile = tileGameObject.GetComponent<Tile>();
                Grid[x, y] = tile;

            }
        }

        //Locks all tiles that are soppused to be locked
        for (int i = 0; i < lockedPosObstacle.Count; i++)
        {
            Grid[(int)lockedPosObstacle[i].x, (int)lockedPosObstacle[i].y].locked = true;
            Debug.Log(lockedPosObstacle[i].x + " - " + lockedPosObstacle[i].y);
        }

        for (int i = 0; i < lockedPosObstacle.Count; i++)
        {
            //Debug.Log(lockedPos[i].x + " - " + lockedPos[i].y);

        }
        if (obstaclePos.GroupBy(n => n).Any(c => c.Count() > 1) || obstaclePos.Any(par => (int)par.x == 0) || obstaclePos.Any(par => (int)par.y == 0))
        {
            Debug.Log("There is duplicate obstacle position or any position has 0");
        }
    }


}
