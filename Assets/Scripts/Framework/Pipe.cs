using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pipe : MonoBehaviour
{
    [SerializeField] private GameObject WeldSparkParticleSystemPrefab;
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
        set
        {
            team = value;
            if (isSource)
            {
                foreach (Transform t in transform.FindChild("Geometry").transform)
                {
                    t.GetComponent<MeshRenderer>().material.color = GameData.TeamColors[value];
                }
            }
            else 
                meshRenderer.material.color = GameData.TeamColors[value];
        }
        get { return team; }
    }

    public List<GameData.Coordinate> connections;
    public GameData.Coordinate positionCoordinate { get; protected set; } 
    public bool isCenterMachine { get; protected set; }
    public bool isSource { get; protected set; }
    public bool isFlameMachine { get; protected set; }

    private List<GameData.Coordinate> visited;
    private List<Pipe> connectedPipes;

    protected MeshRenderer meshRenderer;
    protected PipeMan pipeMan;
    protected GridController gridController;
    private AnimationManager AnimationManager;

    //TODO: REMOVE TEST
    public bool todestroy = false;

    void Start() {
        AnimationManager = GameObject.FindObjectOfType<AnimationManager>();
    }
    void Update()
    {
        if (todestroy) DestroyPipe();
    }

    public void Initialize(PipeData.PipeType pipeType, GameData.Coordinate coord, int rotationAngle) {
        pipeMan = GameController.Instance.PipeMan;
        gridController = GameController.Instance.GridController;
        meshRenderer = GetComponent<MeshRenderer>();
        connections = new List<GameData.Coordinate>();



        PipeType = pipeType;
        positionCoordinate = coord;
        List<GameData.Team> connectedTeams = new List<GameData.Team>();

        //Foreach connection in the pipes default rotation-set, rotate the connection to fit the actual placement.
        foreach (Vector2 v in pipeMan.pipeConnections[pipeType]) {
            Vector2 rotatedVector = Quaternion.Euler(0, 0, -rotationAngle) * v;
            GameData.Coordinate conCoord = new GameData.Coordinate(positionCoordinate.x + Mathf.RoundToInt(rotatedVector.x), positionCoordinate.y + Mathf.RoundToInt(rotatedVector.y));
            if (conCoord.x >= 0 && conCoord.x <= gridController.Grid.GetLength(0) - 1 && conCoord.y >= 0 && conCoord.y <= gridController.Grid.GetLength(1) - 1) {
                connections.Add(conCoord);
                if (gridController.Grid[conCoord.x, conCoord.y].pipe != null)
                {
                    if (!connectedTeams.Contains(gridController.Grid[conCoord.x, conCoord.y].pipe.team))
                        connectedTeams.Add(gridController.Grid[conCoord.x, conCoord.y].pipe.team);
                    float x = 0.5f;
                    if (gridController.Grid[conCoord.x, conCoord.y].pipe.isSource)
                        x = 0.25f;
                    Vector3 sparkPos = Vector3.Lerp(transform.position, gridController.Grid[conCoord.x, conCoord.y].pipe.transform.position, x);
                    GameObject sparks = Instantiate(WeldSparkParticleSystemPrefab, new Vector3(sparkPos.x, 0.5f, sparkPos.z) , Quaternion.Euler(-48.3f, 152.4f, 0)) as GameObject;
                    WeldsparksParticleSystem sparksParticleSystem = sparks.GetComponent<WeldsparksParticleSystem>();
                    bool isHorizontalWeld = false;
                    if (rotatedVector.x > 0.5f || rotatedVector.x < -0.5f)
                        isHorizontalWeld = true;
                    else if (rotatedVector.y > 0.5f || rotatedVector.y < -0.5f)
                        isHorizontalWeld = false;
                    sparksParticleSystem.Initialize(isHorizontalWeld);
                }
            }
        }
        gridController.Grid[coord.x, coord.y].SetPipe(this);

        foreach (GameData.Team t in connectedTeams)
        {
            if (connectedTeams.Count == 1)
            {
                Team = t;
                break;
            }
            else if (connectedTeams.Count == 2 && connectedTeams.Contains(GameData.Team.Neutral))
            {
                TurnConnectedPipesToTeam(connectedTeams.First(x => x != GameData.Team.Neutral));
            }
            else
            {
                //TODO: ADD FUNCTIONALITY FOR MULTIPLE TEAMS CONNECTED TO SAME PIPE
                Team = t;
                break;
            }
        }

        //Animation SpriteSheet Setup
        AnimationManager = GetComponent<AnimationManager>();
        AnimationManager.FindPipeStatus(pipeType, Team);
    }

    public void DestroyPipe() {
        gridController.Grid[positionCoordinate.x, positionCoordinate.y].SetPipe(null);
        foreach (GameData.Coordinate c in connections)
        {
            Pipe connectedPipe = gridController.Grid[c.x, c.y].pipe;
            if (connectedPipe != null)
            {
                if (!connectedPipe.CheckSourceConnection())
                {
                    connectedPipe.TurnConnectedPipesToTeam(GameData.Team.Neutral);
                }
            }
        }
        Destroy(gameObject);
    }

    public bool CheckSourceConnection()
    {
        visited = new List<GameData.Coordinate>();
        return isConnectedToSource(positionCoordinate);
    }

    private bool isConnectedToSource(GameData.Coordinate coord)
    {
        if (isSource) return true;
        if (isCenterMachine) return false;
        List<GameData.Coordinate> cons = gridController.Grid[coord.x, coord.y].pipe.connections;
        visited.Add(coord);
        foreach (GameData.Coordinate c in cons)
        {
            if (c == coord) continue;
            if (gridController.Grid[c.x, c.y].pipe == null) continue;
            if (gridController.Grid[c.x, c.y].pipe.Team != team) continue;
            if (visited.Contains(c)) continue;

            if (gridController.Grid[c.x, c.y].pipe.isSource) return true;
            if (isConnectedToSource(c)) return true;
        }
        return false;
    }

    public void TurnConnectedPipesToTeam(GameData.Team newTeam)
    {
        visited = new List<GameData.Coordinate>();
        connectedPipes = new List<Pipe>();
        connectedPipes.Add(this);
        GetConnectedPipes(positionCoordinate);
        foreach (Pipe pipe in connectedPipes)
        {
            pipe.Team = newTeam;          
        }
    }

    private void GetConnectedPipes(GameData.Coordinate coord)
    {
        List<GameData.Coordinate> cons = gridController.Grid[coord.x, coord.y].pipe.connections;
        visited.Add(coord);
        foreach (GameData.Coordinate c in cons) {
            if (c == coord) continue;
            if (gridController.Grid[c.x, c.y].pipe == null) continue;
            if (gridController.Grid[c.x, c.y].pipe.Team != team && gridController.Grid[c.x, c.y].pipe.Team != GameData.Team.Neutral) continue;
            if (visited.Contains(c)) continue;

            connectedPipes.Add(gridController.Grid[c.x, c.y].pipe);
            GetConnectedPipes(c);
        }
        return;
    }
    
    public void SetHightlight(bool val)
    {
        if(val)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
