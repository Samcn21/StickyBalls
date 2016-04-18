using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;

//Controls input on controllers as well as placing pipe. (It is a bit too highly coupled, I'm sorry.)
public class InputController : MonoBehaviour
{
    [SerializeField] private float stickSensivity = 0.25f;
    [SerializeField] private GamePad.Index index;

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
    private List<int> possibleRotationAngles;
    private int rotationIndex = 0;

    private bool initialized = false;
    
    public GameData.Team team { get; private set; }

    public void Initialize (GameData.Team t, GamePad.Index padIndex)
    {
        team = t;
        gamepadIndex = padIndex;
        gamepadState = GamePad.GetState(gamepadIndex);
        rigidbody = GetComponent<Rigidbody>();
        initialized = true;
    }

	// Use this for initialization
	void Start ()
	{
	    player = GetComponent<Player>();
	    triggerCollider = GetComponent<SphereCollider>();
        closeConveyorPipes = new List<ConveyorPipe>();
        closePipeConnections = new List<GameData.Coordinate>();
        possibleRotationAngles = new List<int>();
	    pipeMan = GameController.Instance.PipeMan;
	    gridController = GameController.Instance.GridController;


        //TEST
        Initialize(GameData.Team.Black, index);
	}

    void FixedUpdate()
    {
        if (!initialized) return;
        gamepadState = GamePad.GetState(gamepadIndex);
        rigidbody.AddForce(new Vector3(gamepadState.LeftStickAxis.x * stickSensivity, 0, gamepadState.LeftStickAxis.y * stickSensivity  ) * player.moveSpeed);
    }
	// Update is called once per frame
	void Update ()
	{
	    if (!initialized) return;

        //If A is pressed and you are currently near a spot where a pipe can be placed, place the pipe
        //If A is pressed and you have a conveyor pipe selected, pick up the conveyor pipe
	    if (GamePad.GetButtonDown(GamePad.Button.A, gamepadIndex))
	    {
	        if (selectedConveyorPipe != null)
	        {
	            player.PickupPipe(selectedConveyorPipe);
	            closeConveyorPipes.Remove(selectedConveyorPipe);
                Destroy(selectedConveyorPipe.gameObject);
                selectedConveyorPipe = null;
	        }
            else if (selectedPipeConnection != null)
            {
                PlacePipe();
            }
	    }

        //If B is pressed, rotate the pipe to one of the allowed rotations
	    if (GamePad.GetButtonDown(GamePad.Button.B, gamepadIndex))
	    {
	        if (selectedPipeConnection != null)
	        {
	            rotationIndex++;
	            if (rotationIndex > possibleRotationAngles.Count - 1)
	                rotationIndex = 0;
                RotatePipe(possibleRotationAngles[rotationIndex]);
	        }
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
            foreach (GameData.Coordinate tile in closePipeConnections) {
                float distance = Vector3.Distance(transform.position, gridController.Grid[tile.x, tile.y].transform.position);
                if (distance < shortestDistance) {
                    shortestDistance = distance;
                    closestPipeConnection = tile;
                }
            }
            //If the selected tile changes, destroy the previous placeholder and place a new.
            if (selectedPipeConnection != closestPipeConnection) {
                if (selectedPipePlaceholder != null) 
                    Destroy(selectedPipePlaceholder.gameObject);
                selectedPipeConnection = closestPipeConnection;
                GameObject placeholder = Instantiate(pipeMan.placeholderPrefab, gridController.Grid[selectedPipeConnection.x, selectedPipeConnection.y].transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
                selectedPipePlaceholder = placeholder.transform;
                selectedPipePlaceholder.GetComponent<MeshRenderer>().material =
                    pipeMan.placeholderPipeTextures[player.HeldPipeType];
                selectedPipePlaceholder.GetComponent<MeshRenderer>().material.color = Color.green;

                possibleRotationAngles = CalculatePossibleRotations(selectedPipeConnection, player.HeldPipeType);
                RotatePipe(possibleRotationAngles[0]);
            }
        }
        
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
            foreach (GameData.Coordinate c in pipe.connections) {
                if (gridController.Grid[c.x, c.y].pipe == null && !closePipeConnections.Contains(c) && !gridController.Grid[c.x, c.y].locked) {
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
        if (conveyorPipe != null) {
            if (closeConveyorPipes.Contains(conveyorPipe)) {
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
                if (closePipeConnections.Count == 0) {
                    selectedPipeConnection = null;
                    if (selectedPipePlaceholder != null)
                        Destroy(selectedPipePlaceholder.gameObject);
                }
            }
        }
    }

    //Calculates the possible rotation that a pipe can have for a point in space. Looks for nearby pipes that are connected to that spot
    //and tries to see which rotations it has, that can fit them.
    private List<int> CalculatePossibleRotations(GameData.Coordinate toPlace, PipeData.PipeType type)
    {
        List<Vector2> rotations = new List<Vector2>();
        List<int> rotationAngles = new List<int>();
        if (toPlace.x > 0)
        {
            if (gridController.Grid[toPlace.x - 1, toPlace.y].pipe != null)
            {
                if (gridController.Grid[toPlace.x - 1, toPlace.y].pipe.connections.Contains(toPlace))
                    rotations.Add(new Vector2(-1,0));
            }
        }
        if (toPlace.x < gridController.Grid.GetLength(0)-1) {
            if (gridController.Grid[toPlace.x + 1, toPlace.y].pipe != null) {
                if (gridController.Grid[toPlace.x + 1, toPlace.y].pipe.connections.Contains(toPlace))
                    rotations.Add(new Vector2(1, 0));
            }
        }
        if (toPlace.y > 0) {
            if (gridController.Grid[toPlace.x, toPlace.y - 1].pipe != null) {
                if (gridController.Grid[toPlace.x, toPlace.y - 1].pipe.connections.Contains(toPlace))
                    rotations.Add(new Vector2(0, -1));
            }
        }
        if (toPlace.y < gridController.Grid.GetLength(1) - 1) {
            if (gridController.Grid[toPlace.x, toPlace.y + 1].pipe != null) {
                if (gridController.Grid[toPlace.x, toPlace.y + 1].pipe.connections.Contains(toPlace))
                    rotations.Add(new Vector2(0, 1));
            }
        }

        List<Vector2> pipeConnections = pipeMan.pipeConnections[type];
        for (int i = 0; i < 4; i++)
        {
            int rotationAngle = i*90;
            foreach (Vector2 v in pipeConnections)
            {
                Vector2 rotated = Quaternion.Euler(0, 0, -rotationAngle) * v;
                foreach (Vector2 rotation in rotations)
                {
                    if (rotation == rotated && !rotationAngles.Contains(rotationAngle))
                        rotationAngles.Add(rotationAngle);
                }
            }
        }
        return rotationAngles;
    }

    private void RotatePipe(int rotationAngle)
    {
        selectedPipePlaceholder.rotation = Quaternion.Euler(90, rotationAngle, 0);
    }

    private void PlacePipe()
    {
        GameObject newPipe = Instantiate(pipeMan.pipePrefab,
                       gridController.Grid[selectedPipeConnection.x, selectedPipeConnection.y].transform.position,
                           Quaternion.Euler(90, possibleRotationAngles[rotationIndex], 0)) as GameObject;
        Pipe pipe = newPipe.GetComponent<Pipe>();
        pipe.Initialize(player.HeldPipeType, selectedPipeConnection, possibleRotationAngles[rotationIndex]);
        player.PlacePipe();
        selectedPipeConnection = null;
        closePipeConnections.Remove(selectedPipeConnection);
        Destroy(selectedPipePlaceholder.gameObject);
        rotationIndex = 0;
    }
}
