﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using System.Linq;
using UnityEngine.SceneManagement;

//Controls input on controllers as well as placing pipe. (It is a bit too highly coupled, I'm sorry.)
public class InputController : MonoBehaviour
{
    [SerializeField] private float stickSensivity = 0.25f;
    [SerializeField] private float velocityThreshold = 0.1f;
    [SerializeField] private float holdTimerLimit = 1.25f;
    public GamePad.Index index;
    public GameData.Team team;

    //TODO: Remove animation and sprite variables after instalation of new animation system
    public RuntimeAnimatorController redAnim;
    public RuntimeAnimatorController blueAnim;
    public RuntimeAnimatorController yellowAnim;
    public RuntimeAnimatorController blackAnim;
    public Sprite redSprite;
    public Sprite blueSprite;
    public Sprite yellowSprite;
    public Sprite blackSprite;

    private AudioManager AudioManager;
    private Player player;
    private GamepadState gamepadState;
    private GamePad.Index gamepadIndex;
    private PipeMan pipeMan;
    private GridController gridController;
    private Rigidbody rigidBody;
    private List<ConveyorPipe> closeConveyorPipes;
    private ConveyorPipe selectedConveyorPipe;
    private List<GameData.Coordinate> closePipeConnections;
    private GameData.Coordinate selectedPipeConnection;
    private PipeStatus pipeStatus;
    private Transform selectedPipePlaceholder;
    private int rotationIndex = 0;
    private bool initialized = false;
    private bool isLegalRotation = false;
    private List<Pipe> closePipes;
    public GameData.Direction characterFacing = GameData.Direction.South;
    public bool colorPicked { get; private set; }
    public bool colorPickPermit = false;
    private Pipe pipeToDestroyRef = null;
    public CharacterSprite CharacterSprite { get; private set; }

    //Checking the velocity of the player
    private float velocityX;
    private float velocityZ;
    private float velocityTotal;
    private float destroyTimer;
    private float resetDestroyTimer;
    private bool isPressingX;
    private float holdTimer = 0;

    public bool isPressingDelete { get; private set; }
    public bool isDead { get; private set; }
    public bool isLocked;


    //TEST VARIABLES
    [SerializeField] private bool TEST_DELETE = false;

    public void Initialize(GameData.Team t, GamePad.Index padIndex)
    {
        team = t;
        gamepadIndex = padIndex;
        gamepadState = GamePad.GetState(gamepadIndex);
        rigidBody = GetComponent<Rigidbody>();
        ColorInit(t);  //Color initialization 
        initialized = true;
        colorPicked = false;
        isPressingDelete = false;
        resetDestroyTimer = GameController.Instance.PipeStatus.TimerToDestroyPipe;
        player.Initialize();
    }

    // Use this for initialization
    void Start()
    {
        AudioManager = GameObject.FindObjectOfType<AudioManager>();
        CharacterSprite = GetComponentInChildren<CharacterSprite>();
        player = GetComponent<Player>();
        closeConveyorPipes = new List<ConveyorPipe>();
        closePipeConnections = new List<GameData.Coordinate>();
        pipeMan = GameController.Instance.PipeMan;
        gridController = GameController.Instance.GridController;
        pipeStatus = GameController.Instance.PipeStatus;
        AssignColorsToPlayers();
        closePipes = new List<Pipe>();
        isPressingX = false;
        destroyTimer = resetDestroyTimer;
        isDead = false;
        isLocked = false;
    }


    void FixedUpdate()
    {
        if (isLocked)
            return;
        Rigidbody playerRigidbody = GetComponent<Rigidbody>();
        velocityX = playerRigidbody.velocity.x;
        velocityZ = playerRigidbody.velocity.z;
        velocityTotal = Mathf.Abs(velocityX + velocityZ);

        //This method check player's states for movement animation sound and player facing 
        PlayerState();

        if (!initialized) return;
        gamepadState = GamePad.GetState(gamepadIndex);

        //TODO: These lines must be removed after adding new animation system
        float velocity = rigidBody.velocity.x + rigidBody.velocity.z;
        Animator myAnim = GetComponentInChildren<Animator>();
        if (myAnim != null)
        {
            myAnim.SetFloat("velocity", Mathf.Abs(velocity));
        }
        if (holdTimer >= 0)
            rigidBody.AddForce(new Vector3(gamepadState.LeftStickAxis.x * stickSensivity, 0, gamepadState.LeftStickAxis.y * stickSensivity) * player.moveSpeed);
    }

    void Update()
    {
        if (!initialized || isLocked) return;
        
        if (pipeToDestroyRef != null && player.HeldPipeType == PipeData.PipeType.Void)
            pipeToDestroyRef.SetHightlight(true);


        if (TEST_DELETE)
        {
            if (GameController.Instance.PipeStatus.DestroySinglePipeActive && GamePad.GetButtonDown(GamePad.Button.X, gamepadIndex)) {
                isPressingX = true;
                isPressingDelete = true;
            }
            else {
                isPressingDelete = false;
            }

            if (GameController.Instance.PipeStatus.DestroySinglePipeActive && GamePad.GetButtonUp(GamePad.Button.X, gamepadIndex)) {
                isPressingX = false;
                destroyTimer = resetDestroyTimer;
            }
            if (isPressingX && pipeToDestroyRef != null) {
                destroyTimer -= Time.deltaTime;
                if (pipeToDestroyRef != null && destroyTimer <= 0) {
                    pipeStatus.DestroyPipeOfPlayer(team, pipeToDestroyRef, true);
                    pipeToDestroyRef = null;
                }
            }
        }

        if (gamepadState.A)
        {
            if (pipeToDestroyRef != null && !TEST_DELETE && selectedConveyorPipe == null && selectedPipeConnection == null) {
                if (holdTimer >= holdTimerLimit) {
                    PickUpPipe(pipeToDestroyRef);
                    holdTimer = 0;
                }
                else
                    holdTimer += Time.deltaTime;
            }
        }

        //If A is pressed and you are currently near a spot where a pipe can be placed, place the pipe
        //Else If A is pressed and you have a conveyor pipe selected, pick up the conveyor pipe
        //Else If A is pressed and you are not holding pipe, or near a conveyor pipe, you pick up a pipe that is already placed
        //If A is pressed and you are in Player Color Assign and collide activate the trigger you will pick the choosen color
        if (GamePad.GetButtonDown(GamePad.Button.A, gamepadIndex))
        {
            if (selectedConveyorPipe != null)
            {
                PickUpPipe(selectedConveyorPipe);
                holdTimer = 0;
            }
            else if (selectedPipeConnection != null)
            {
                if (isLegalRotation)
                    PlacePipe();
                holdTimer = 0;
            }
            else
                holdTimer = 0;

            //color pick 
            if (colorPickPermit && team == GameData.Team.Neutral)
            {
                colorPicked = true;
            }
        }

        //If B is pressed, rotate the pipe to one of the allowed rotations
        if (GamePad.GetButtonDown(GamePad.Button.B, gamepadIndex))
        {
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
        else if (closePipeConnections.Count > 0 && player.HeldPipeType != PipeData.PipeType.Void && player.HeldPipeType != PipeData.PipeType.Dynamite)
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
        else if (player.HeldPipeType == PipeData.PipeType.Dynamite)
        {
            float shortestDistance = float.MaxValue;
            GameData.Coordinate closestPipeConnection = null;
            for (int x = 0; x < gridController.GridWidth; x++)
            {
                for (int y = 0; y < gridController.GridHeight; y++)
                {
                    GameData.Coordinate tile = new GameData.Coordinate(x, y);
                    float distance = Vector3.Distance(transform.position, gridController.Grid[x, y].transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestPipeConnection = tile;
                    }
                }

            }
            if (selectedPipeConnection != closestPipeConnection)
            {
                if (selectedPipePlaceholder != null)
                    Destroy(selectedPipePlaceholder.gameObject);
                selectedPipeConnection = closestPipeConnection;
                if (closestPipeConnection == null)
                    return;
                GameObject placeholder = Instantiate(pipeMan.placeholderPrefab, gridController.Grid[selectedPipeConnection.x, selectedPipeConnection.y].transform.position, Quaternion.Euler(90, rotationIndex * 90, 0)) as GameObject;
                selectedPipePlaceholder = placeholder.transform;
                selectedPipePlaceholder.GetComponent<MeshRenderer>().material = pipeMan.placeholderPipeTextures[player.HeldPipeType];
                selectedPipePlaceholder.GetComponent<MeshRenderer>().material.color = Color.green;
                isLegalRotation = true;
            }
        }
        if (closePipes.Count == 0)
            closePipeConnections.Clear();
    }

    void OnTriggerStay(Collider col)
    {
        OnTriggerEnter(col);
    }

    //If player is not holding a pipe, check if the collider is a conveyor pipe
    //Else check if it was a pipe, and get it's connections where you could possible place the pipe you're holding
    void OnTriggerEnter(Collider col)
    {
        if (GameController.Instance.PipeStatus.DestroySinglePipeActive && col.gameObject.tag == "Pipe")
        {
            Pipe pipe = col.gameObject.GetComponent<Pipe>();
            if (pipe.Team != team) {
                goto next;
            }
            if (pipeToDestroyRef == null)
            {
                pipeToDestroyRef = pipe;
            }
            else
            {
                if (Mathf.Abs(Vector3.Distance(transform.position, col.gameObject.transform.position)) <
                    Mathf.Abs(Vector3.Distance(transform.position, pipeToDestroyRef.gameObject.transform.position)))
                {
                    pipeToDestroyRef.SetHightlight(false);
                    pipeToDestroyRef = col.GetComponent<Pipe>();
                    pipeToDestroyRef.SetHightlight(true);
                }
            }
        }
next:   
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
                    if (!closePipes.Contains(pipe))
                        closePipes.Add(pipe);
                    closePipeConnections.Add(c);
                }
            }
        }
    }

    //If it was a conveyor pipe, remove the selection. 
    //If it was a pipe remove it.
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Pipe"&&pipeToDestroyRef==col.GetComponent<Pipe>())
        {
            pipeToDestroyRef.SetHightlight(false);
            pipeToDestroyRef = null;
        }
            

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
        //SFX
        //AudioManager.PlayOneShotPlayer(GameData.AudioClipState.RotatePipe, index, true);

        rotationIndex++;
        if (rotationIndex > 3)
            rotationIndex = 0;
        player.RotatePipe(rotationIndex * 90);
        if (selectedPipeConnection == null)
            return;
        selectedPipePlaceholder.rotation = Quaternion.Euler(90, rotationIndex * 90, 0);
        isLegalRotation = IsLegalRotation(selectedPipeConnection, player.HeldPipeType);
        selectedPipePlaceholder.GetComponent<MeshRenderer>().material.color = isLegalRotation ? Color.green : Color.red;
    }

    private void PlacePipe()
    {
        //Animation
        CharacterSprite.FindPlacePipeAnimation();

        //SFX
        AudioManager.PlayOneShotPlayer(GameData.AudioClipState.PlacePipe, index, true);
        Vector3 pipeOffset = new Vector3(0, 1, 0);
        Vector3 dynamiteOffset = new Vector3(0, 2, 0);
        if (player.HeldPipeType != PipeData.PipeType.Dynamite)
        {
            GameObject newPipe = Instantiate(pipeMan.pipePrefab,
                           gridController.Grid[selectedPipeConnection.x, selectedPipeConnection.y].transform.position,
                               Quaternion.Euler(90, rotationIndex * 90, 0)) as GameObject;
            Pipe pipe = newPipe.GetComponent<Pipe>();
            pipe.Initialize(player.HeldPipeType, selectedPipeConnection, rotationIndex * 90);
            bool found = false;
            foreach (Pipe father in closePipes)
            {
                found = false;
                if (father.connections.Contains(selectedPipeConnection)&&father.PipeType!=PipeData.PipeType.Void)
                {
                    found = true;
                    pipeStatus.AddPipeToTeam(pipe.Team, pipe, father);
                    closePipes = new List<Pipe>();
                    break;
                }
            }
            if(!found)
                pipeStatus.AddFirstPipe(pipe.Team, pipe);
            player.PlacePipe();
            selectedPipeConnection = null;
            closePipeConnections.Remove(selectedPipeConnection);
            Destroy(selectedPipePlaceholder.gameObject);
            rotationIndex = 0;
        }
        else
        {
            GameObject newDynamite = Instantiate(pipeMan.dynamitePrefab,
                           gridController.Grid[selectedPipeConnection.x, selectedPipeConnection.y].transform.position,
                               Quaternion.Euler(90, rotationIndex * 90, 0)) as GameObject;
            Dynamite dynamite = newDynamite.GetComponent<Dynamite>();
            dynamite.Initialize(selectedPipeConnection, rotationIndex * 90);
            player.PlacePipe();
            Destroy(selectedPipePlaceholder.gameObject);
        }
        if (pipeToDestroyRef != null)
            pipeToDestroyRef.SetHightlight(false);
        pipeToDestroyRef = null;
    }

    private void PickUpPipe()
    {
        //TODO Animation and sound for pipe pickup
        Animator myAnim = GetComponentInChildren<Animator>();

        if (myAnim != null)
        {
            myAnim.SetTrigger("grabPipe");
        }

        //Animation
        CharacterSprite.FindGrabPipeAnimation();

        //SFX
        AudioManager.PlayOneShotPlayer(GameData.AudioClipState.PickupPipe, index, true);
    }

    private void PickUpPipe(ConveyorPipe conveyorPipe)
    {
        PickUpPipe();
        conveyorPipe.PickPipe();
        player.PickupPipe(conveyorPipe.PipeType, rotationIndex * 90);
        closeConveyorPipes.Remove(selectedConveyorPipe);
        Destroy(selectedConveyorPipe.gameObject);
        selectedConveyorPipe = null;
    }

    private void PickUpPipe(Pipe pipeToPick)
    {
        PickUpPipe();
        player.PickupPipe(pipeToPick.PipeType, Mathf.RoundToInt(pipeToPick.transform.rotation.y));
        pipeToDestroyRef = null;
        pipeStatus.DestroyPipeOfPlayer(pipeToPick.Team, pipeToPick, false);

    }

    private void PlayerState()
    {
        if (CharacterSprite.hasMovementPermit)
        {
            CharacterSprite.previousAnim = CharacterSprite.currentAnim;

            if (velocityTotal <= velocityThreshold)
            {
                if (CharacterSprite.previousAnim.ToString().Contains("Front"))
                {
                    CharacterSprite.currentAnim = GameData.PlayerState.IdleFront;
                }
                else if (CharacterSprite.previousAnim.ToString().Contains("Right"))
                {
                    CharacterSprite.currentAnim = GameData.PlayerState.IdleRight;
                }
                else if (CharacterSprite.previousAnim.ToString().Contains("Left"))
                {
                    CharacterSprite.currentAnim = GameData.PlayerState.IdleLeft;
                }
                else if (CharacterSprite.previousAnim.ToString().Contains("Back"))
                {
                    CharacterSprite.currentAnim = GameData.PlayerState.IdleBack;
                }
            }
            else
            {
                if (velocityX >= velocityThreshold && Mathf.Abs(velocityX) > Mathf.Abs(velocityZ))
                {
                    characterFacing = GameData.Direction.East;
                    CharacterSprite.currentAnim = GameData.PlayerState.MovementRight;
                }
                else if (velocityZ >= velocityThreshold && Mathf.Abs(velocityX) < Mathf.Abs(velocityZ))
                {
                    characterFacing = GameData.Direction.North;
                    CharacterSprite.currentAnim = GameData.PlayerState.MovementBack;
                }
                else if (velocityZ <= velocityThreshold && Mathf.Abs(velocityX) < Mathf.Abs(velocityZ))
                {
                    characterFacing = GameData.Direction.South;
                    CharacterSprite.currentAnim = GameData.PlayerState.MovementFront;
                }
                else if (velocityX <= velocityThreshold && Mathf.Abs(velocityX) > Mathf.Abs(velocityZ))
                {
                    characterFacing = GameData.Direction.West;
                    CharacterSprite.currentAnim = GameData.PlayerState.MovementLeft;
                }
            }
        }
    }

    void ColorInit(GameData.Team t)
    {
        Animator myAnim = GetComponentInChildren<Animator>();
        SpriteRenderer mySprite = GetComponentInChildren<SpriteRenderer>();
        GameObject purpleSpot = GameObject.Find("Purple");
        GameObject blueSpot = GameObject.Find("Blue");
        GameObject yellowSpot = GameObject.Find("Yellow");
        GameObject cyanSpot = GameObject.Find("Cyan");
        
        //I'm using new animation system don't need this part, I'll replace this part as soon as new system replaced
        if (mySprite != null)
        {
            //change the color of the player in order to the gamePad index number and move the player
            // to the related respawn spots (next to their source) that have the same color as the player
            if (t.ToString().Contains("Purple"))
            {
                mySprite.sprite = redSprite;
                myAnim.runtimeAnimatorController = redAnim;
                transform.position = purpleSpot.transform.position;
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
            else if (t.ToString().Contains("Cyan"))
            {
                mySprite.sprite = blackSprite;
                myAnim.runtimeAnimatorController = blackAnim;
                transform.position = cyanSpot.transform.position;
            }
        }
    }

    void AssignColorsToPlayers()
    {
        //Gives default color to the players
        if (SceneManager.GetActiveScene().name != "PlayerColorAssign")
        {
            //If PlayerPrefs in not null, it means that either PlayerColorAssign level has been played before 
            //or this level invokes directly from PlayerColorAssign level

            if (PlayerPrefs.GetInt("isDataSaved") == 1 && !GameController.Instance.Gamemode_IsCoop)
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
                Dictionary<GamePad.Index, GameData.Team> defaultPlayerIndexColor = null;
                if (GameController.Instance.Gamemode_IsCoop)
                {
                    defaultPlayerIndexColor = new Dictionary<GamePad.Index, GameData.Team>()
                    {
                        {GamePad.Index.One, GameData.Team.Cyan},
                        {GamePad.Index.Two, GameData.Team.Cyan},
                        {GamePad.Index.Three, GameData.Team.Purple},
                        {GamePad.Index.Four, GameData.Team.Purple}
                    };
                }
                else
                {
                //if playerPrefs is null then assign this colors to the players
                    defaultPlayerIndexColor = new Dictionary<GamePad.Index, GameData.Team>()
                    {
                        {GamePad.Index.One, GameData.Team.Cyan},
                        {GamePad.Index.Two, GameData.Team.Yellow},
                        {GamePad.Index.Three, GameData.Team.Blue},
                        {GamePad.Index.Four, GameData.Team.Purple}
                    };
                }
                

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

    public void Die()
    {
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
    }
}
