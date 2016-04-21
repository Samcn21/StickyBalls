using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    
    [SerializeField] private Transform boardGenerationPoint;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int gridHeight = 9;
    [SerializeField] private int gridWidth = 16;

    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }

    public Tile[,] Grid;
	// Use this for initialization
	void Start ()
	{
	    Grid = new Tile[gridWidth, gridHeight];
        GameObject boardObject = GameObject.Find("BOARDGENERATIONPOINT");

        if (boardObject != null) {
            GenerateGrid(boardGenerationPoint.position);
        }
        
	    GridWidth = gridWidth;
	    GridHeight = gridHeight;
	}
	
	// Update is called once per frame
	void Update () {
	
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


        Grid[middleX - 2, middleY + 2].locked = true;
        Grid[middleX - 1, middleY + 2].locked = true;
        Grid[middleX + 1, middleY + 2].locked = true;
        Grid[middleX + 2, middleY + 2].locked = true;

        Grid[middleX - 2, middleY + 1].locked = true;
        Grid[middleX - 1, middleY + 1].locked = true;
        Grid[middleX, middleY + 1].locked = true;
        Grid[middleX + 1, middleY + 1].locked = true;
        Grid[middleX + 2, middleY + 1].locked = true;

        Grid[middleX - 1, middleY].locked = true;
        Grid[middleX, middleY].locked = true;
        Grid[middleX + 1, middleY].locked = true;
        
        Grid[middleX - 2, middleY - 1].locked = true;
        Grid[middleX - 1, middleY - 1].locked = true;
        Grid[middleX, middleY - 1].locked = true;
        Grid[middleX + 1, middleY - 1].locked = true;
        Grid[middleX + 2, middleY - 1].locked = true;

        Grid[middleX - 2, middleY - 2].locked = true;
        Grid[middleX - 1, middleY - 2].locked = true;
        Grid[middleX + 1, middleY - 2].locked = true;
        Grid[middleX + 2, middleY - 2].locked = true;



    }
}
