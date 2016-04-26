using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using System.Linq;

//Controls input on controllers as well as placing pipe. (It is a bit too highly coupled, I'm sorry.)
public class InputController : MonoBehaviour
{
    [SerializeField]
    private float stickSensivity = 0.25f;
    public GamePad.Index index;
    public GameData.Team team;
    public RuntimeAnimatorController redAnim;
    public RuntimeAnimatorController blueAnim;
    public RuntimeAnimatorController yellowAnim;
    public RuntimeAnimatorController blackAnim;
    public Sprite redSprite;
    public Sprite blueSprite;
    public Sprite yellowSprite;
    public Sprite blackSprite;
    private Player player;
    private GamepadState gamepadState;
    private GamePad.Index gamepadIndex;
    private PipeMan pipeMan;
    private GridController gridController;
    private Rigidbody rigidbody;
    private SphereCollider triggerCollider;
    private List<ConveyorPipe> closeConveyorPipes;
    private ConveyorPipe selectedConveyorPipe;
    private List<GameData.Coordinate> closePipeConnections;
    private GameData.Coordinate selectedPipeConnection;
    private Transform selectedPipePlaceholder;
    private int rotationIndex = 0;
    private bool initialized = false;
    private bool isLegalRotation = false;
    public bool colorPicked { get; private set; }
    public bool colorPickPermit = false;


    public void Initialize(GameData.Team t, GamePad.Index padIndex)
    {
        Animator myAnim = GetComponentInChildren<Animator>();
        SpriteRenderer mySprite = GetComponentInChildren<SpriteRenderer>();
        GameObject redSpot = GameObject.Find("Red");
        GameObject blueSpot = GameObject.Find("Blue");
        GameObject yellowSpot = GameObject.Find("Yellow");
        GameObject balckSpot = GameObject.Find("Black");

        //change the color of the player in order to the gamePad index number and move the player
        // to the related respawn spots (next to their source) that have the same color as the player
        if (t.ToString().Contains("Red"))
        {
            mySprite.sprite = redSprite;
            myAnim.runtimeAnimatorController = redAnim;
            transform.position = redSpot.transform.position;
        }
        else if (t.ToString().Contains("Blue"))
        {
            mySprite.sprite = blueSprite;
            myAnim.runtimeAnimatorController = blueAnim;
            transform.position = blueSpot.transform.position;
        }
        else if (t.ToString().Contains("Yellow"))
        {
            mySprite.sprite = yellowSprite;
            myAnim.runtimeAnimatorController = yellowAnim;
            transform.position = yellowSpot.transform.position;
        }
        else if (t.ToString().Contains("Black"))
        {
            mySprite.sprite = blackSprite;
            myAnim.runtimeAnimatorController = blackAnim;
            transform.position = balckSpot.transform.position;
        }

        team = t;
        gamepadIndex = padIndex;
        gamepadState = GamePad.GetState(gamepadIndex);
        rigidbody = GetComponent<Rigidbody>();
        initialized = true;
        colorPicked = false;
    }

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
        triggerCollider = GetComponent<SphereCollider>();
        closeConveyorPipes = new List<ConveyorPipe>();
        closePipeConnections = new List<GameData.Coordinate>();
        pipeMan = GameController.Instance.PipeMan;
        gridController = GameController.Instance.GridController;
        AssignColorsToPlayers();
    }
    void AssignColorsToPlayers()
    {
        //Gives default color to the players
        if ((Application.loadedLevelName.ToString() != "PlayerColorAssign"))
        {
            //If PlayerPrefs in not null, it means that either PlayerColorAssign level has been played before 
            //or this level invokes directly from PlayerColorAssign level
            if (PlayerPrefs.GetInt("isDataSaved") == 1)
            {
                string[] names = Enum.GetNames(typeof(GamePad.Index));
                Dictionary<GamePad.Index, GameData.Team> playerPrefIndexColor = new Dictionary<GamePad.Index, GameData.Team>();
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] != GamePad.Index.Any.ToString())
                    {
                        if (PlayerPrefs.GetString(names[i]) != null)
                        {
                            GameData.Team colorValue = (GameData.Team)Enum.Parse(typeof(GameData.Team), PlayerPrefs.GetString(names[i]));
                            GamePad.Index indexValue = (GamePad.Index)Enum.Parse(typeof(GamePad.Index), names[i]);
                            playerPrefIndexColor[indexValue] = colorValue;
                        }
                    }
                }

                foreach (KeyValuePair<GamePad.Index, GameData.Team> eachPlayer in playerPrefIndexColor)
                {
                    if (eachPlayer.Key == index)
                    {
                        Initialize(eachPlayer.Value, index);
                    }
                }
            }
            else
            {
                //if playerPrefs is null then assign this colors to the players
                Dictionary<GamePad.Index, GameData.Team> defaultPlayerIndexColor = new Dictionary<GamePad.Index, GameData.Team>() 
                {
                    {GamePad.Index.One, GameData.Team.Red},
                    {GamePad.Index.Two, GameData.Team.Yellow},
                    {GamePad.Index.Three, GameData.Team.Blue},
                    {GamePad.Index.Four, GameData.Team.Black},
                };

                foreach (KeyValuePair<GamePad.Index, GameData.Team> eachPlayer in defaultPlayerIndexColor)
                {
                    if (eachPlayer.Key == index)
                    {
                        Initialize(eachPlayer.Value, index);
                    }
                }
            }
        }
        else
        {
            //in Player Color Assign page gives everybody Neutral color!
            Initialize(GameData.Team.Neutral, index);
        }
    }

    void FixedUpdate()
    {
        if (!initialized) return;
        gamepadState = GamePad.GetState(gamepadIndex);
        
        float velocity = rigidbody.velocity.x + rigidbody.velocity.z;
        Animator myAnim = GetComponentInChildren<Animator>();
        myAnim.SetFloat("velocity", Mathf.Abs(velocity));

        rigidbody.AddForce(new Vector3(gamepadState.LeftStickAxis.x * stickSensivity, 0, gamepadState.LeftStickAxis.y * stickSensivity) * player.moveSpeed);
    }

    void Update()
    {
        if (!initialized) return;
        
        //If A is pressed and you are currently near a spot where a pipe can be placed, place the pipe
        //If A is pressed and you have a conveyor pipe selected, pick up the conveyor pipe
        //If A is pressed and you are in Player Color Assign and collide activate the trigger you will pick the choosen color
        if (GamePad.GetButtonDown(GamePad.Button.A, gamepadIndex))
        {
            
            if (selectedConveyorPipe != null)
            {
                //TODO Animation and sound for pipe pickup
                Animator myAnim = GetComponentInChildren<Animator>();
                myAnim.SetTrigger("grabPipe");

                player.PickupPipe(selectedConveyorPipe);
                closeConveyorPipes.Remove(selectedConveyorPipe);
                Destroy(selectedConveyorPipe.gameObject);
                selectedConveyorPipe = null;
            }
            else if (selectedPipeConnection != null)
            {
                if (isLegalRotation)
                    PlacePipe();
            }


            //color pick 
            if (colorPickPermit && team == GameData.Team.Neutral)
            {
                colorPicked = true;
            }
        }

        //If B is pressed, rotate the pipe to one of the allowed rotations
        if (GamePad.GetButtonDown(GamePad.Button.B, gamepadIndex)) {
            if (player.HeldPipeType != PipeData.PipeType.Void) 
                RotatePipe();
        }

        //Select closest conveyor pipe out of all within the sphere collider
        if (closeConveyorPipes.Count > 0 && player.HeldPipeType == PipeData.PipeType.Void)
        {
            float shortestDistance = float.MaxValue;
            ConveyorPipe closestPipe = null;
            List<ConveyorPipe> pipesToDelete = new List<ConveyorPipe>();
            foreach (ConveyorPipe pipe in closeConveyorPipes)
            {
                if (pipe == null)
                {
                    pipesToDelete.Add(pipe);
                    continue;
                }
                float distance = Vector3.Distance(transform.position, pipe.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPipe = pipe;
                }
            }
            foreach (ConveyorPipe pipe in pipesToDelete)
            {
                closeConveyorPipes.Remove(pipe);
            }
            if (selectedConveyorPipe != closestPipe)
            {
                if (selectedConveyorPipe != null)
                    selectedConveyorPipe.SetHightlight(false);
                selectedConveyorPipe = closestPipe;
                closestPipe.SetHightlight(true);
            }
        }

        //Select closest place to put a pipe out of all within the sphere collider
        else if (closePipeConnections.Count > 0 && player.HeldPipeType != PipeData.PipeType.Void)
        {
            float shortestDistance = float.MaxValue;
            GameData.Coordinate closestPipeConnection = null;
            foreach (GameData.Coordinate tile in closePipeConnections)
            {
                if (gridController.Grid[tile.x, tile.y].pipe != null)
                    continue;
                float distance = Vector3.Distance(transform.position, gridController.Grid[tile.x, tile.y].transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPipeConnection = tile;
                }
            }
            //If the selected tile changes, destroy the previous placeholder and place a new.
            if (selectedPipeConnection != closestPipeConnection)
            {

                if (selectedPipePlaceholder != null)
                    Destroy(selectedPipePlaceholder.gameObject);
                selectedPipeConnection = closestPipeConnection;
                if (closestPipeConnection == null)
                    return;
                GameObject placeholder = Instantiate(pipeMan.placeholderPrefab, gridController.Grid[selectedPipeConnection.x, selectedPipeConnection.y].transform.position, Quaternion.Euler(90, rotationIndex * 90, 0)) as GameObject;
                selectedPipePlaceholder = placeholder.transform;
                selectedPipePlaceholder.GetComponent<MeshRenderer>().material =
                    pipeMan.placeholderPipeTextures[player.HeldPipeType];
                selectedPipePlaceholder.GetComponent<MeshRenderer>().material.color = Color.green;

                isLegalRotation = IsLegalRotation(selectedPipeConnection, player.HeldPipeType);
                selectedPipePlaceholder.GetComponent<MeshRenderer>().material.color = isLegalRotation ? Color.green : Color.red;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        OnTriggerEnter(col);
    }

    //If player is not holding a pipe, check if the collider is a conveyor pipe
    //Else check if it was a pipe, and get it's connections where you could possible place the pipe you're holding
    void OnTriggerEnter(Collider col)
    {
        if (player.HeldPipeType == PipeData.PipeType.Void)
        {
            ConveyorPipe conveyorPipe = col.gameObject.GetComponent<ConveyorPipe>();
            if (conveyorPipe != null)
            {
                if (!closeConveyorPipes.Contains(conveyorPipe))
                {
                    closeConveyorPipes.Add(conveyorPipe);
                }
            }
        }
        else
        {
            Pipe pipe = col.GetComponent<Pipe>();
            if (pipe == null) return;
            if (pipe.Team == GameData.Team.Neutral && !pipe.isCenterMachine)
                return;
            foreach (GameData.Coordinate c in pipe.connections)
            {
                if (gridController.Grid[c.x, c.y].pipe == null && !closePipeConnections.Contains(c) && !gridController.Grid[c.x, c.y].locked)
                {
                    closePipeConnections.Add(c);
                }
            }
        }
    }

    //If it was a conveyor pipe, remove the selection. 
    //If it was a pipe remove it.
    void OnTriggerExit(Collider col)
    {
        ConveyorPipe conveyorPipe = col.gameObject.GetComponent<ConveyorPipe>();
        if (conveyorPipe != null)
        {
            if (closeConveyorPipes.Contains(conveyorPipe))
            {
                closeConveyorPipes.Remove(conveyorPipe);
                if (closeConveyorPipes.Count == 0)
                {
                    if (selectedConveyorPipe != null)
                    {
                        selectedConveyorPipe.SetHightlight(false);
                        selectedConveyorPipe = null;
                    }
                }
            }
        }

        Pipe pipe = col.GetComponent<Pipe>();
        if (pipe == null) return;
        foreach (GameData.Coordinate c in pipe.connections)
        {
            if (closePipeConnections.Contains(c))
            {
                closePipeConnections.Remove(c);
                if (closePipeConnections.Count == 0)
                {
                    selectedPipeConnection = null;
                    if (selectedPipePlaceholder != null)
                        Destroy(selectedPipePlaceholder.gameObject);
                }
            }
        }
    }

    //Calculates the possible rotation that a pipe can have for a point in space. Looks for nearby pipes that are connected to that spot
    //and tries to see which rotations it has, that can fit them.
    private bool IsLegalRotation(GameData.Coordinate toPlace, PipeData.PipeType type)
    {
        List<Vector2> rotations = new List<Vector2>();
        if (toPlace.x > 0)
        {
            if (gridController.Grid[toPlace.x - 1, toPlace.y].pipe != null)
            {
                if (gridController.Grid[toPlace.x - 1, toPlace.y].pipe.connections.Contains(toPlace) && !gridController.Grid[toPlace.x - 1, toPlace.y].pipe.isCenterMachine)
                    rotations.Add(new Vector2(-1, 0));
            }
        }
        if (toPlace.x < gridController.Grid.GetLength(0) - 1)
        {
            if (gridController.Grid[toPlace.x + 1, toPlace.y].pipe != null)
            {
                if (gridController.Grid[toPlace.x + 1, toPlace.y].pipe.connections.Contains(toPlace) && !gridController.Grid[toPlace.x + 1, toPlace.y].pipe.isCenterMachine)
                    rotations.Add(new Vector2(1, 0));
            }
        }
        if (toPlace.y > 0)
        {
            if (gridController.Grid[toPlace.x, toPlace.y - 1].pipe != null)
            {
                if (gridController.Grid[toPlace.x, toPlace.y - 1].pipe.connections.Contains(toPlace) && !gridController.Grid[toPlace.x, toPlace.y - 1].pipe.isCenterMachine)
                    rotations.Add(new Vector2(0, -1));
            }
        }
        if (toPlace.y < gridController.Grid.GetLength(1) - 1)
        {
            if (gridController.Grid[toPlace.x, toPlace.y + 1].pipe != null)
            {
                if (gridController.Grid[toPlace.x, toPlace.y + 1].pipe.connections.Contains(toPlace) && !gridController.Grid[toPlace.x, toPlace.y + 1].pipe.isCenterMachine)
                    rotations.Add(new Vector2(0, 1));
            }
        }

        List<Vector2> pipeConnections = pipeMan.pipeConnections[type];

        int rotationAngle = rotationIndex * 90;
        foreach (Vector2 v in pipeConnections)
        {
            Vector2 rotated = Quaternion.Euler(0, 0, -rotationAngle) * v;
            foreach (Vector2 rotation in rotations)
            {
                if (rotation == rotated)
                    return true;
            }
        }
        return false;
    }

    private void RotatePipe()
    {
        rotationIndex++;
        if (rotationIndex > 3)
            rotationIndex = 0;
        if (selectedPipeConnection == null)
            return;
        selectedPipePlaceholder.rotation = Quaternion.Euler(90, rotationIndex * 90, 0);
        isLegalRotation = IsLegalRotation(selectedPipeConnection, player.HeldPipeType);
        selectedPipePlaceholder.GetComponent<MeshRenderer>().material.color = isLegalRotation ? Color.green : Color.red;
    }

    private void PlacePipe()
    {
        GameObject newPipe = Instantiate(pipeMan.pipePrefab,
                       gridController.Grid[selectedPipeConnection.x, selectedPipeConnection.y].transform.position,
                           Quaternion.Euler(90,rotationIndex * 90, 0)) as GameObject;
        Pipe pipe = newPipe.GetComponent<Pipe>();
        pipe.Initialize(player.HeldPipeType, selectedPipeConnection, rotationIndex * 90);
        player.PlacePipe();
        selectedPipeConnection = null;
        closePipeConnections.Remove(selectedPipeConnection);
        Destroy(selectedPipePlaceholder.gameObject);
        rotationIndex = 0;
    }

}
