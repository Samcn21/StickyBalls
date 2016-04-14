using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        switch (sourceLocation)
        {
            case GameData.PlayerSourceDirection.BottomLeft:
                me = new GameData.Coordinate(1, 2);
                con = new GameData.Coordinate(1, 3);
                me2 = new GameData.Coordinate(2, 1);
                con2 = new GameData.Coordinate(3, 1);
                break;
            case GameData.PlayerSourceDirection.BottomRight:
                me = new GameData.Coordinate(gridController.Grid.GetLength(0)-3, 1);
                con = new GameData.Coordinate(gridController.Grid.GetLength(0)-4, 1);
                me2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, 2);
                con2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, 3);
                break;
            case GameData.PlayerSourceDirection.TopLeft:
                me = new GameData.Coordinate(2, gridController.Grid.GetLength(1)-2);
                con = new GameData.Coordinate(3, gridController.Grid.GetLength(1)-2);
                me2 = new GameData.Coordinate(1, gridController.Grid.GetLength(1)-3);
                con2 = new GameData.Coordinate(1, gridController.Grid.GetLength(1)-4);
                break;
            case GameData.PlayerSourceDirection.TopRight:
                me = new GameData.Coordinate(gridController.Grid.GetLength(0)-3, gridController.Grid.GetLength(1)-2);
                con = new GameData.Coordinate(gridController.Grid.GetLength(0)-4, gridController.Grid.GetLength(1)-2);
                me2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, gridController.Grid.GetLength(1)-3);
                con2 = new GameData.Coordinate(gridController.Grid.GetLength(0)-2, gridController.Grid.GetLength(1)-4);
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
