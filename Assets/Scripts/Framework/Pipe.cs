using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pipe : MonoBehaviour
{
    private PipeData.PipeType pipeType;
    public PipeData.PipeType PipeType
    {
        private set
        {
            meshRenderer.material = pipeMan.pipeTextures[value];
            pipeType = value;
        }
        get { return pipeType; }
    }

    private GameData.Team team;
    public GameData.Team Team {
        private set { team = value; }
        get { return team; }
    }

    public List<GameData.Coordinate> connections;
    public GameData.Coordinate positionCoordinate { get; private set; } 

    private MeshRenderer meshRenderer;
    protected PipeMan pipeMan;
    protected GridController gridController;

    public void Initialize(PipeData.PipeType pipeType, GameData.Coordinate coord, int rotationAngle) {
        pipeMan = GameController.Instance.PipeMan;
        gridController = GameController.Instance.GridController;
        meshRenderer = GetComponent<MeshRenderer>();
        connections = new List<GameData.Coordinate>();
        PipeType = pipeType;
        positionCoordinate = coord;

        foreach (Vector2 v in pipeMan.pipeConnections[pipeType]) {
            Vector2 rotatedVector = Quaternion.Euler(0, 0, -rotationAngle) * v;
            GameData.Coordinate conCoord = new GameData.Coordinate(positionCoordinate.x + Mathf.RoundToInt(rotatedVector.x), positionCoordinate.y + Mathf.RoundToInt(rotatedVector.y));
            connections.Add(conCoord);
        }
        gridController.Grid[coord.x, coord.y].SetPipe(this);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
