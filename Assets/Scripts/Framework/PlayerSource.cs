using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//The corner piece. It extends Pipe, so that you can connect to it, but it is own, so it can be checked against for win conditions.
public class PlayerSource : Pipe
{
    public GameData.PlayerSourceDirection sourceLocation;

    public GameData.Coordinate[] coordinateOfOutingPoints { get; private set; }
    private List<GameData.Coordinate> lockedTiles;
    private List<GameData.Coordinate> openTiles;
    private GameData.Coordinate me;
    private GameData.Coordinate me2;

    //State Machine
    private StateManager StateManager;
    private GameObject gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        StateManager = gameController.GetComponent<StateManager>();

        coordinateOfOutingPoints = new GameData.Coordinate[2];
        gridController = GameController.Instance.GridController;
        connections = new List<GameData.Coordinate>();
        meshRenderer = GetComponent<MeshRenderer>();
        lockedTiles = new List<GameData.Coordinate>();
        openTiles = new List<GameData.Coordinate>();
        
        this.isSource = true;
        GameData.Coordinate con = null;
        GameData.Coordinate con2 = null;
        //Depending on where the machine spawns, it needs to add connections to coordinates that is available to place a pipe on
        //It also locks the pieces that is under it.
        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            if (GameController.Instance.Gamemode_IsCoop)
            {
                switch (sourceLocation)
                {
                    case GameData.PlayerSourceDirection.BottomLeft:
                        me = new GameData.Coordinate(1, 2);
                        con = new GameData.Coordinate(1, 3);
                        me2 = new GameData.Coordinate(2, 1);
                        con2 = new GameData.Coordinate(3, 1);
                        lockedTiles.Add(new GameData.Coordinate(0, 0));
                        lockedTiles.Add(new GameData.Coordinate(0, 1));
                        lockedTiles.Add(new GameData.Coordinate(1, 0));
                        lockedTiles.Add(new GameData.Coordinate(1, 1));
                        lockedTiles.Add(new GameData.Coordinate(0, 2));
                        lockedTiles.Add(new GameData.Coordinate(2, 0));
                        lockedTiles.Add(new GameData.Coordinate(2, 2));
                        Team = GameData.Team.Purple;
                        break;
                    case GameData.PlayerSourceDirection.TopRight:
                        me = new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 2);
                        con = new GameData.Coordinate(gridController.Grid.GetLength(0) - 4, gridController.Grid.GetLength(1) - 2);
                        me2 = new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 3);
                        con2 = new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 4);
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 2));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 3));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 2));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 3));
                        Team = GameData.Team.Cyan;
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                }
            }
            else
            {
                switch (sourceLocation)
                {
                    case GameData.PlayerSourceDirection.BottomLeft:
                        me = new GameData.Coordinate(1, 2);
                        con = new GameData.Coordinate(1, 3);
                        me2 = new GameData.Coordinate(2, 1);
                        con2 = new GameData.Coordinate(3, 1);
                        lockedTiles.Add(new GameData.Coordinate(0, 0));
                        lockedTiles.Add(new GameData.Coordinate(0, 1));
                        lockedTiles.Add(new GameData.Coordinate(1, 0));
                        lockedTiles.Add(new GameData.Coordinate(1, 1));
                        lockedTiles.Add(new GameData.Coordinate(0, 2));
                        lockedTiles.Add(new GameData.Coordinate(2, 0));
                        lockedTiles.Add(new GameData.Coordinate(2, 2));
                        Team = GameData.Team.Purple;

                        break;
                    case GameData.PlayerSourceDirection.BottomRight:
                        me = new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, 1);
                        con = new GameData.Coordinate(gridController.Grid.GetLength(0) - 4, 1);
                        me2 = new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, 2);
                        con2 = new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, 3);
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, 0));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, 2));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, 0));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, 0));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, 2));
                        Team = GameData.Team.Blue;
                        break;
                    case GameData.PlayerSourceDirection.TopLeft:
                        me = new GameData.Coordinate(2, gridController.Grid.GetLength(1) - 2);
                        con = new GameData.Coordinate(3, gridController.Grid.GetLength(1) - 2);
                        me2 = new GameData.Coordinate(1, gridController.Grid.GetLength(1) - 3);
                        con2 = new GameData.Coordinate(1, gridController.Grid.GetLength(1) - 4);
                        lockedTiles.Add(new GameData.Coordinate(0, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(0, gridController.Grid.GetLength(1) - 2));
                        lockedTiles.Add(new GameData.Coordinate(0, gridController.Grid.GetLength(1) - 3));
                        lockedTiles.Add(new GameData.Coordinate(1, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(1, gridController.Grid.GetLength(1) - 2));
                        lockedTiles.Add(new GameData.Coordinate(2, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(2, gridController.Grid.GetLength(1) - 3));
                        Team = GameData.Team.Cyan;
                        break;
                    case GameData.PlayerSourceDirection.TopRight:
                        me = new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 2);
                        con = new GameData.Coordinate(gridController.Grid.GetLength(0) - 4, gridController.Grid.GetLength(1) - 2);
                        me2 = new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 3);
                        con2 = new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 4);
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 2));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 1, gridController.Grid.GetLength(1) - 3));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 2, gridController.Grid.GetLength(1) - 2));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 1));
                        lockedTiles.Add(new GameData.Coordinate(gridController.Grid.GetLength(0) - 3, gridController.Grid.GetLength(1) - 3));
                        Team = GameData.Team.Yellow;
                        break;
                }
            }

            
        }
        else if (StateManager.CurrentActiveState == GameData.GameStates.ColorAssignFFA) 
        {


            openTiles.Add(new GameData.Coordinate(12, 6));
            openTiles.Add(new GameData.Coordinate(13, 6));
            openTiles.Add(new GameData.Coordinate(14, 6));
            openTiles.Add(new GameData.Coordinate(10, 4));
            openTiles.Add(new GameData.Coordinate(10, 3));
            openTiles.Add(new GameData.Coordinate(10, 2));
            openTiles.Add(new GameData.Coordinate(10, 8));
            openTiles.Add(new GameData.Coordinate(10, 9));
            openTiles.Add(new GameData.Coordinate(10, 10));
            openTiles.Add(new GameData.Coordinate(6, 6));
            openTiles.Add(new GameData.Coordinate(7, 6));
            openTiles.Add(new GameData.Coordinate(8, 6));

            GridController GridController = gameController.GetComponent<GridController>();
            switch (sourceLocation)
            {
                case GameData.PlayerSourceDirection.BottomLeft:
                    me = new GameData.Coordinate(10, 2);
                    con = new GameData.Coordinate(10, 3);
                    Team = GameData.Team.Purple;
                    break;

                case GameData.PlayerSourceDirection.BottomRight:
                    me = new GameData.Coordinate(14, 6);
                    con = new GameData.Coordinate(13, 6);
                    Team = GameData.Team.Blue;
                    break;

                case GameData.PlayerSourceDirection.TopLeft:
                    me = new GameData.Coordinate(6, 6);
                    con = new GameData.Coordinate(7, 6);
                    Team = GameData.Team.Cyan;
                    break;

                case GameData.PlayerSourceDirection.TopRight:
                    me = new GameData.Coordinate(10, 10);
                    con = new GameData.Coordinate(10, 9);
                    Team = GameData.Team.Yellow;
                    break;
            }

            for (int x = 0; x < GridController.GridWidth; x++)
            {
                for (int y = 0; y < GridController.GridHeight; y++)
                {
                    lockedTiles.Add(new GameData.Coordinate(x, y));
                }
            }

            lockedTiles = lockedTiles.Except(openTiles).ToList();
        }

        foreach (GameData.Coordinate coordinate in lockedTiles)
        {
            gridController.Grid[coordinate.x, coordinate.y].locked = true;
        }

        connections.Add(con);
        if (con2 != null)
        {
            connections.Add(con2);
        }

        coordinateOfOutingPoints[0] = new GameData.Coordinate(me.x, me.y);
        gridController.Grid[me.x, me.y].SetPipe(this);

        if (me2 != null)
        {
            coordinateOfOutingPoints[1] = new GameData.Coordinate(me2.x, me2.y);
            gridController.Grid[me2.x, me2.y].SetPipe(this);
        }
        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            GameController.Instance.PlayerSources.Add(Team, this);
        }
    }

    public void Explode()
    {
        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            gridController.Grid[me.x, me.y].SetPipe(null);
            gridController.Grid[me2.x, me2.y].SetPipe(null);
            foreach (GameData.Coordinate coordinate in lockedTiles)
            {
                gridController.Grid[coordinate.x, coordinate.y].locked = false;
            }

            Instantiate(GameController.Instance.PipeStatus.bigExplosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
