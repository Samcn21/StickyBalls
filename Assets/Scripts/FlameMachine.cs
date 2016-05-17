using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameMachine : Pipe {
    void Start()
    {
        gridController = GameController.Instance.GridController;
    }

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
        if (xBase + 1 < gridController.GridWidth)
        {
            con = new GameData.Coordinate(xBase + 1, yBase);
            connections.Add(con);
        }
        if (yBase + 1 < gridController.GridHeight)
        {
            con2 = new GameData.Coordinate(xBase, yBase + 1);
            connections.Add(con2);
        }
        if (xBase - 1 >= 0)
        {
            con3 = new GameData.Coordinate(xBase - 1, yBase);
            connections.Add(con3);
        }
        if (yBase - 1 >= 0)
        {
            con4 = new GameData.Coordinate(xBase, yBase - 1);
            connections.Add(con4);
        }
        gridController.Grid[xBase, yBase].locked = true;
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
                    foreach (GameData.Coordinate coord in gridController.Grid[connection.x, connection.y].pipe.connections)
                        if (coord.Equals(positionCoordinate))
                        {
                            GameController.Instance.GridController.Grid[connection.x, connection.y].pipe.DestroyFlameMachine();
                            break;
                        }
                }
            }
        }
    }
}
