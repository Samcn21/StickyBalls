using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CenterMachine : Pipe
{

	// Use this for initialization
	void Start () {
        connections = new List<GameData.Coordinate>();
        gridController = GameController.Instance.GridController;
        int middleX = Mathf.FloorToInt(gridController.GridWidth / 2);
        int middleY = Mathf.FloorToInt(gridController.GridHeight / 2);

	    gridController.Grid[middleX - 2, middleY].SetPipe(this);
        gridController.Grid[middleX, middleY + 2].SetPipe(this);
        gridController.Grid[middleX + 2, middleY].SetPipe(this);
        gridController.Grid[middleX, middleY -2].SetPipe(this);

        connections.Add(new GameData.Coordinate(middleX, middleY + 3));
        connections.Add(new GameData.Coordinate(middleX + 3, middleY));
        connections.Add(new GameData.Coordinate(middleX, middleY - 3));
        connections.Add(new GameData.Coordinate(middleX - 3, middleY));
    }
	
}
