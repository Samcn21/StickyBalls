using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Data;

public class CenterMachine : Pipe
{
    // Use this for initialization
    private List<GameData.Coordinate> localConnectionCoordinates; 

    void Start () {
        localConnectionCoordinates = new List<GameData.Coordinate>();
        connections = new List<GameData.Coordinate>();
        gridController = GameController.Instance.GridController;
        meshRenderer = GetComponent<MeshRenderer>();
        int middleX = Mathf.FloorToInt(gridController.GridWidth / 2);
        int middleY = Mathf.FloorToInt(gridController.GridHeight / 2);
	    this.isCenterMachine = true;
        Team = GameData.Team.Neutral;

	    gridController.Grid[middleX - 2, middleY].SetPipe(this);
        localConnectionCoordinates.Add(new GameData.Coordinate(middleX - 2, middleY));
        gridController.Grid[middleX, middleY + 2].SetPipe(this);
        localConnectionCoordinates.Add(new GameData.Coordinate(middleX, middleY + 2));
        gridController.Grid[middleX + 2, middleY].SetPipe(this);
        localConnectionCoordinates.Add(new GameData.Coordinate(middleX + 2, middleY));
        gridController.Grid[middleX, middleY -2].SetPipe(this);
        localConnectionCoordinates.Add(new GameData.Coordinate(middleX, middleY - 2));

        connections.Add(new GameData.Coordinate(middleX, middleY + 3));
        connections.Add(new GameData.Coordinate(middleX + 3, middleY));
        connections.Add(new GameData.Coordinate(middleX, middleY - 3));
        connections.Add(new GameData.Coordinate(middleX - 3, middleY));
    }

    void Update()
    {
        foreach (GameData.Coordinate connection in connections)
        {
            if (gridController.Grid[connection.x, connection.y].pipe != null)
            {
                if (gridController.Grid[connection.x, connection.y].pipe.Team != GameData.Team.Neutral && localConnectionCoordinates.Any(gridController.Grid[connection.x, connection.y].pipe.connections.Contains))
                {
                    if (gridController.Grid[connection.x, connection.y].pipe.CheckSourceConnection())
                        GameController.Instance.PlayerWon(gridController.Grid[connection.x, connection.y].pipe.Team);
                }
            }
        }
    }
	
}
