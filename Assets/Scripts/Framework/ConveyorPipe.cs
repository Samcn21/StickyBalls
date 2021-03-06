﻿using UnityEngine;
using System.Collections;

public class ConveyorPipe : MonoBehaviour
{
    public PipeData.PipeType PipeType = PipeData.PipeType.Void;

    [SerializeField]
    private float moveSpeed = 0.5f;
    private ConveyorBelt conveyorBelt;
    private Transform prevTravelTarget;
    private float moveProgress = 0;
    private bool initialized = false;
    private PipeMan pipeMan;

    public Transform currentTravelTarget { get; private set; }
    public int travelPointIndex = 0;

    //For testing picking up
    [SerializeField]
    private bool Pick = false;

    //State Machine
    private StateManager StateManager;
    private GameObject gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        StateManager = gameController.GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            //Gets distance to next travel point on conveyor belt. If close enough, it will request the next travel point on the belt, and travel to that.
            //If next travelpoint is not empty, it will be stuck polling for next, until it's empty.
            float distanceToTravelPoint = Vector3.Distance(transform.position, currentTravelTarget.position);
            if (distanceToTravelPoint < 0.0001f)
            {
                Transform newTravelTarget = conveyorBelt.GetNextTravelPoint(this);

                if (newTravelTarget != null)
                {
                    prevTravelTarget = currentTravelTarget;
                    currentTravelTarget = newTravelTarget;
                    if (travelPointIndex + 1 > conveyorBelt.pipesOnBelt.Length - 1)
                        travelPointIndex = 0;
                    else 
                        travelPointIndex++;
                    moveProgress = 0;
                }
            }
            else
            {
                moveProgress += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(prevTravelTarget.position, currentTravelTarget.position, moveProgress);
            }

            if (Pick) PickPipe();
        }
    }

    //Initializes ConveyorPipe with the correct material and type.
    public void Initialize(PipeData.PipeType pipeType, Transform travelTarget, int travelIndex, ConveyorBelt belt)
    {
        travelPointIndex = travelIndex;
        pipeMan = GameController.Instance.PipeMan;
        PipeType = pipeType;
        conveyorBelt = belt;
        GetComponent<MeshRenderer>().material = pipeMan.pipeTextures[PipeType];
        currentTravelTarget = travelTarget;
        initialized = true;
    }

    //Should return pipe that you can place, and remove this pipe from the belt.
    public void PickPipe()
    {
        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            conveyorBelt.PickConveyorPipe(travelPointIndex);
        }
        Destroy(gameObject);
    }

    public void SetHightlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
   
}
