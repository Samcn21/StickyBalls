﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//The corner piece. It extends Pipe, so that you can connect to it, but it is own, so it can be checked against for win conditions.
public class PlayerSource : Pipe
{
    [SerializeField] private GameData.PlayerSourceDirection sourceLocation;

    // Use this for initialization
    void Start ()
    {
        gridController = GameController.Instance.GridController;
	    connections = new List<GameData.Coordinate>();

        GameData.Coordinate me = null;
        GameData.Coordinate me2 = null;
        GameData.Coordinate con = null;
        GameData.Coordinate con2 = null;
        //Depending on where the machine spawns, it needs to add connections to coordinates that is available to place a pipe on
        //It also locks the pieces that is under it.
        switch (sourceLocation)
        {
            case GameData.PlayerSourceDirection.BottomLeft:
                me = new GameData.Coordinate(1, 2);
                con = new GameData.Coordinate(1, 3);
                me2 = new GameData.Coordinate(2, 1);
                con2 = new GameData.Coordinate(3, 1);
                gridController.Grid[0, 0].locked = true;
                gridController.Grid[0, 1].locked = true;
                gridController.Grid[1, 0].locked = true;
                gridController.Grid[1, 1].locked = true;
                gridController.Grid[0, 2].locked = true;
                gridController.Grid[2, 0].locked = true;
                gridController.Grid[2, 2].locked = true;

                break;
            case GameData.PlayerSourceDirection.BottomRight:
                me = new GameData.Coordinate(gridController.Grid.GetLength(0)-3, 1);
                con = new GameData.Coordinate(gridController.Grid.GetLength(0)-4, 1);
                me2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, 2);
                con2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, 3);
                gridController.Grid[gridController.Grid.GetLength(0) - 1, 0].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 1, 1].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 1, 2].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 2, 0].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 2, 1].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 3, 0].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 3, 2].locked = true;
                break;
            case GameData.PlayerSourceDirection.TopLeft:
                me = new GameData.Coordinate(2, gridController.Grid.GetLength(1)-2);
                con = new GameData.Coordinate(3, gridController.Grid.GetLength(1)-2);
                me2 = new GameData.Coordinate(1, gridController.Grid.GetLength(1)-3);
                con2 = new GameData.Coordinate(1, gridController.Grid.GetLength(1)-4);
                gridController.Grid[0, gridController.Grid.GetLength(1) - 1].locked = true;
                gridController.Grid[0, gridController.Grid.GetLength(1) - 2].locked = true;
                gridController.Grid[0, gridController.Grid.GetLength(1) - 3].locked = true;
                gridController.Grid[1, gridController.Grid.GetLength(1) - 1].locked = true;
                gridController.Grid[1, gridController.Grid.GetLength(1) - 2].locked = true;
                gridController.Grid[2, gridController.Grid.GetLength(1) - 1].locked = true;
                gridController.Grid[2, gridController.Grid.GetLength(1) - 3].locked = true;
                break;
            case GameData.PlayerSourceDirection.TopRight:
                me = new GameData.Coordinate(gridController.Grid.GetLength(0)-3, gridController.Grid.GetLength(1)-2);
                con = new GameData.Coordinate(gridController.Grid.GetLength(0)-4, gridController.Grid.GetLength(1)-2);
                me2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, gridController.Grid.GetLength(1)-3);
                con2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, gridController.Grid.GetLength(1)-4);
                gridController.Grid[gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 1].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 2].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 3].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 1].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 2].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 1].locked = true;
                gridController.Grid[gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 3].locked = true;
                break;
        }
        connections.Add(con);
        connections.Add(con2);
        gridController.Grid[me.x, me.y].SetPipe(this);
        gridController.Grid[me2.x, me2.y].SetPipe(this);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}