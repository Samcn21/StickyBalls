﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{

    [SerializeField]
    private Transform boardGenerationPoint;
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private int gridHeight = 9;
    [SerializeField]
    private int gridWidth = 16;

    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }
    public Tile[,] Grid;
    public List<Vector2> lockedTiles = new List<Vector2>();

    //State Machine
    private StateManager StateManager;
    private GameObject gameController;

    void Awake()
    {
        enabled = !GameController.Instance.isPregame;
        if (!enabled) return;
        Grid = new Tile[gridWidth, gridHeight];
        GameObject boardObject = GameObject.Find("BOARDGENERATIONPOINT");

        if (boardObject != null)
        {
            GenerateGrid(boardGenerationPoint.position);
        }

        GridWidth = gridWidth;
        GridHeight = gridHeight;

        ObstacleController ObstacleController = GetComponent<ObstacleController>();
        if (ObstacleController != null)
        {
            ObstacleController.LockTiles(lockedTiles);
        }
    }

    public void GenerateGrid(Vector3 originPoint)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject tileGameObject = Instantiate(tilePrefab, new Vector3(originPoint.x + x, originPoint.y, originPoint.z + y), Quaternion.Euler(90, 0, 0)) as GameObject;
                tileGameObject.name = x + "," + y;
                tileGameObject.transform.parent = transform;
                Tile tile = tileGameObject.GetComponent<Tile>();
                Grid[x, y] = tile;
            }
        }


        int middleX = Mathf.FloorToInt(gridWidth / 2);
        int middleY = Mathf.FloorToInt(gridHeight / 2);

        gameController = GameObject.FindGameObjectWithTag("GameController");
        StateManager = gameController.GetComponent<StateManager>();

        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            //Lock center tiles
            lockedTiles.Add(new Vector2(middleX - 2, middleY + 2));
            lockedTiles.Add(new Vector2(middleX - 1, middleY + 2));
            lockedTiles.Add(new Vector2(middleX + 1, middleY + 2));
            lockedTiles.Add(new Vector2(middleX + 2, middleY + 2));

            lockedTiles.Add(new Vector2(middleX - 2, middleY + 1));
            lockedTiles.Add(new Vector2(middleX - 1, middleY + 1));
            lockedTiles.Add(new Vector2(middleX, middleY + 1));
            lockedTiles.Add(new Vector2(middleX + 1, middleY + 1));
            lockedTiles.Add(new Vector2(middleX + 2, middleY + 1));

            lockedTiles.Add(new Vector2(middleX - 1, middleY));
            lockedTiles.Add(new Vector2(middleX, middleY));
            lockedTiles.Add(new Vector2(middleX + 1, middleY));

            lockedTiles.Add(new Vector2(middleX - 2, middleY - 1));
            lockedTiles.Add(new Vector2(middleX - 1, middleY - 1));
            lockedTiles.Add(new Vector2(middleX, middleY - 1));
            lockedTiles.Add(new Vector2(middleX + 1, middleY - 1));
            lockedTiles.Add(new Vector2(middleX + 2, middleY - 1));

            lockedTiles.Add(new Vector2(middleX - 2, middleY - 2));
            lockedTiles.Add(new Vector2(middleX - 1, middleY - 2));
            lockedTiles.Add(new Vector2(middleX + 1, middleY - 2));
            lockedTiles.Add(new Vector2(middleX + 2, middleY - 2));

            //it's not necessary to lock here! it will send this list to obstacleController class and obstacle controller 
            //will also lock all locklist (including central machine , sources and obstacles if there are any)
            for (int i = 0; i < lockedTiles.Count; i++)
            {
                Grid[(int)lockedTiles[i].x, (int)lockedTiles[i].y].locked = true;
            }
        }

    }

    public bool IsInsideGrid(GameData.Coordinate coord)
    {
        return (coord.x < GridWidth && coord.x >= 0 && coord.y < GridHeight && coord.y >= 0);
    }
}
