using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    
    [SerializeField] private Transform boardGenerationPoint;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int gridHeight = 9;
    [SerializeField] private int gridWidth = 16;

    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }
    public Tile[,] Grid;
    public List<Vector2> lockedPos = new List<Vector2>(); 
	
    void Start ()
	{
	    Grid = new Tile[gridWidth, gridHeight];
        GameObject boardObject = GameObject.Find("BOARDGENERATIONPOINT");

        if (boardObject != null) {
            GenerateGrid(boardGenerationPoint.position);
        }
        
	    GridWidth = gridWidth;
	    GridHeight = gridHeight;

        ObstacleController ObstacleController = GetComponent<ObstacleController>();
        ObstacleController.Initialize(lockedPos);
	}
	
    public void GenerateGrid(Vector3 originPoint)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject tileGameObject = Instantiate(tilePrefab, new Vector3(originPoint.x + x,originPoint.y, originPoint.z + y), Quaternion.Euler(90, 0, 0)) as GameObject;
                tileGameObject.name = x + "," + y;
                tileGameObject.transform.parent = transform;
                Tile tile = tileGameObject.GetComponent<Tile>();
                Grid[x, y] = tile;
            }
        }

        
        int middleX = Mathf.FloorToInt(gridWidth / 2);
        int middleY = Mathf.FloorToInt(gridHeight / 2);

        lockedPos.Add(new Vector2(middleX - 2, middleY + 2));
        lockedPos.Add(new Vector2(middleX - 1, middleY + 2));
        lockedPos.Add(new Vector2(middleX + 1, middleY + 2));
        lockedPos.Add(new Vector2(middleX + 2, middleY + 2));

        lockedPos.Add(new Vector2(middleX - 2, middleY + 1));
        lockedPos.Add(new Vector2(middleX - 1, middleY + 1));
        lockedPos.Add(new Vector2(middleX    , middleY + 1));
        lockedPos.Add(new Vector2(middleX + 1, middleY + 1));
        lockedPos.Add(new Vector2(middleX + 2, middleY + 1));

        lockedPos.Add(new Vector2(middleX - 1, middleY    ));
        lockedPos.Add(new Vector2(middleX    , middleY    ));
        lockedPos.Add(new Vector2(middleX + 1, middleY    ));

        lockedPos.Add(new Vector2(middleX - 2, middleY - 1));
        lockedPos.Add(new Vector2(middleX - 1, middleY - 1));
        lockedPos.Add(new Vector2(middleX    , middleY - 1));
        lockedPos.Add(new Vector2(middleX + 1, middleY - 1));
        lockedPos.Add(new Vector2(middleX + 2, middleY - 1));

        lockedPos.Add(new Vector2(middleX - 2, middleY - 2));
        lockedPos.Add(new Vector2(middleX - 1, middleY - 2));
        lockedPos.Add(new Vector2(middleX + 1, middleY - 2));
        lockedPos.Add(new Vector2(middleX + 2, middleY - 2));

        //not necessary to lock here! it will send this list to obstacleController and there it will lock all locklist!
        for (int i = 0; i < lockedPos.Count; i++)
        {
            Grid[(int)lockedPos[i].x, (int)lockedPos[i].y].locked = true;
        }
        
    }
}
