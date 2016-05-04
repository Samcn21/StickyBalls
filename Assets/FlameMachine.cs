using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameMachine : Pipe {

    public FlameMachinePosition position;

    void Start()
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
        int xBase=0;
        int yBase = 0;
        switch (position)
        {
            case FlameMachinePosition.Left:
                xBase = gridController.GridWidth / 8;
                yBase = gridController.GridHeight / 2;
                break;
            case FlameMachinePosition.Right:
                xBase = gridController.GridWidth * 7 / 8;
                yBase = gridController.GridHeight / 2;
                break;
        }
        me = new GameData.Coordinate(xBase,yBase);
        con = new GameData.Coordinate(xBase+1, yBase);
        con2 = new GameData.Coordinate(xBase, yBase+1);
        con3 = new GameData.Coordinate(xBase-1, yBase);
        con4 = new GameData.Coordinate(xBase, yBase-1);
        gridController.Grid[gridController.GridWidth / 4, gridController.GridHeight / 2].locked = true;
        connections.Add(con);
        connections.Add(con2);
        connections.Add(con3);
        connections.Add(con4);
        gridController.Grid[me.x, me.y].SetPipe(this);
        transform.position = gridController.Grid[me.x, me.y].gameObject.transform.position;
        positionCoordinate = me;
    }
    public enum FlameMachinePosition
    {
        Right,Left
    }

    void Update()
    {
        foreach (GameData.Coordinate connection in connections)
        {
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
