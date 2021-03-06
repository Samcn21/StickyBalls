﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 1f;
    [SerializeField]
    private Transform heldPipe;
    [SerializeField]
    private Transform heldPipeContainer;
    [SerializeField]
    private GameObject[] cryParticles;

    private MeshRenderer heldPipeRenderer;
    private PipeData.PipeType heldPipeType;
    private PipeMan pipeMan;
    private InputController InputController;

    public bool isDead { get; private set; }
    public GameData.Team Team { get; private set; }

    //State Machine
    private StateManager StateManager;
    void Start()
    {
        StateManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StateManager>();
        pipeMan = GameController.Instance.PipeMan;
        heldPipeRenderer = heldPipe.GetComponent<MeshRenderer>();
        InputController = GetComponent<InputController>();
        heldPipe.parent = null;
        foreach (GameObject g in cryParticles)
            g.SetActive(false);
    }

    public void EnableCryParticles()
    {
        foreach (GameObject g in cryParticles)
            g.SetActive(true);
    }

    void Update()
    {
        Vector3 heldPos = Vector3.zero;
        switch (InputController.characterFacing)
        {
            case GameData.Direction.East:
                heldPos = new Vector3(1, 0.5f, 0);
                break;
            case GameData.Direction.North:
                heldPos = new Vector3(0, 0.5f, 1);
                break;
            case GameData.Direction.West:
                heldPos = new Vector3(-1, 0.5f, 0);
                break;
            case GameData.Direction.South:
                heldPos = new Vector3(0, 0.5f, -1);
                break;
        }
        heldPipeContainer.localPosition = heldPos;
        heldPipe.position = heldPipeContainer.position;
    }


    public PipeData.PipeType HeldPipeType
    {
        get
        {
            return heldPipeType;
        }
        private set
        {
            heldPipeRenderer.enabled = (value != PipeData.PipeType.Void);
            heldPipeRenderer.material = pipeMan.pipeTextures[value];
            heldPipeType = value;
        }
    }

    public void PickupPipe(PipeData.PipeType type, int angle)
    {
        HeldPipeType = type;
        heldPipe.rotation = Quaternion.Euler(90, angle, 0);
    }

    public void PlacePipe()
    {
        HeldPipeType = PipeData.PipeType.Void;
    }

    public void RotatePipe(int angle)
    {
        heldPipe.rotation = Quaternion.Euler(90, angle, 0);
    }

    public void Die()
    {
        InputController.Die();
        isDead = true;
        moveSpeed = moveSpeed / 2;

        foreach (ParticleSystem inChild in GetComponentsInChildren<ParticleSystem>())
        {
            inChild.Play();
        }
    }

    public void Lose()
    {
        InputController.Lose();
    }

    public void Initialize()
    {
        if (GameController.Instance.StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            if(InputController==null)
                InputController = GetComponent<InputController>();
            if (!GameController.Instance.Gamemode_IsCoop)
                GameController.Instance.Players.Add(InputController.team, this);
            else
                GameController.Instance.PlayersCoop[InputController.team].Add(this);
            Team = InputController.team;
        }

    }
}
