using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameMachine : Pipe {
    
    public void Instantiate(GameData.Coordinate coords)
    {
        isFlameMachine = true;
        gridController = GameController.Instance.GridController;
        connections = new List<GameData.Coordinate>();
        meshRenderer = GetComponent<MeshRenderer>();
        Team = GameData.Team.Neutral;
        GameData.Coordinate me = null;
        GameData.Coordinate con = null;
        GameData.Coordinate con2 = null;
        GameData.Coordinate con3 = null;
        GameData.Coordinate con4 = null;
        int xBase = coords.x;
        int yBase = coords.y;
        me = new GameData.Coordinate(xBase, yBase);
        con = new GameData.Coordinate(xBase + 1, yBase);
        con2 = new GameData.Coordinate(xBase, yBase + 1);
        con3 = new GameData.Coordinate(xBase - 1, yBase);
        con4 = new GameData.Coordinate(xBase, yBase - 1);
        gridController.Grid[xBase, yBase].locked = true;
        connections.Add(con);
        connections.Add(con2);
        connections.Add(con3);
        connections.Add(con4);
        gridController.Grid[me.x, me.y].SetPipe(this);
        transform.position = gridController.Grid[me.x, me.y].gameObject.transform.position;
        positionCoordinate = me;
    }

    void Update()
    {
        foreach (GameData.Coordinate connection in connections)
        {
            if (!gridController.IsInsideGrid(connection))
                continue;
            if (gridController.Grid[connection.x, connection.y].pipe != null)
            {
                if (gridController.Grid[connection.x, connection.y].pipe.Team != GameData.Team.Neutral)
                {
                    GameController.Instance.PipeStatus.DestroyPipesFromFlameMachine(positionCoordinate, gridController.Grid[connection.x, connection.y].pipe.Team);
                }
            }
        }
    }
}
