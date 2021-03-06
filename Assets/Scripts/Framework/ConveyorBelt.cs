﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    //Inspector Variables
    [SerializeField] private List<Transform> travelPoints;
    //Template for pipes being spat out on belt
    [SerializeField] private List<PipeData.PipeType> pipeQueueTemplate;
    [SerializeField] private float pipeSpawnInterval = 5;
    [SerializeField] private GameObject conveyorPipePrefab;

    public Transform[] pipesOnBelt { get; protected set; }
    private float _pipeSpawnIntervalRemaining = 0;
    private Queue<PipeData.PipeType> pipeQueue;
    private float counter = 0;

	void Start () {
        pipeQueue = new Queue<PipeData.PipeType>(pipeQueueTemplate.ToArray());
        pipesOnBelt = new Transform[travelPoints.Count];
    }
	
	void Update ()
	{
        if (_pipeSpawnIntervalRemaining <= 0) {
            PipeData.PipeType newType = PipeData.PipeType.Void;
            if (pipesOnBelt[0] == null || pipesOnBelt[pipesOnBelt.Length/2] == null) {
                newType = pipeQueue.Dequeue();
                if (pipeQueue.Count == 0) pipeQueue = new Queue<PipeData.PipeType>(pipeQueueTemplate.ToArray());
            }

            if (pipesOnBelt[0] == null) {
                SpawnPipeTop(newType);
            }
            if (pipesOnBelt[pipesOnBelt.Length / 2] == null) {
                SpawnPipeBottom(newType);
           } 

        }
        else
            _pipeSpawnIntervalRemaining -= Time.deltaTime;
    }

    //Spawns pipe on belt on the first spot, and initializes based on which pipe in the queue it is.
    //If queue is empty, it loops it.
    protected void SpawnPipeTop(PipeData.PipeType type)
    {

        _pipeSpawnIntervalRemaining = pipeSpawnInterval;
        GameObject newPipe = Instantiate(conveyorPipePrefab, travelPoints[0].position, Quaternion.Euler(90, 0, 180)) as GameObject;
        newPipe.GetComponent<ConveyorPipe>().Initialize(type, travelPoints[0], 0, this);
        newPipe.name = counter.ToString();
        counter++;
        pipesOnBelt[0] = newPipe.transform;
    }

    protected void SpawnPipeBottom(PipeData.PipeType type) {
        GameObject newPipe = Instantiate(conveyorPipePrefab, travelPoints[8].position, Quaternion.Euler(90, 0, 180)) as GameObject;
        newPipe.GetComponent<ConveyorPipe>().Initialize(type, travelPoints[8], 8, this);
        newPipe.name = counter.ToString();
        counter++;
        pipesOnBelt[pipesOnBelt.Length / 2] = newPipe.transform;

    }

    //Returns the next travelpoint on the belt, if the position is empty, else null.
    public Transform GetNextTravelPoint(ConveyorPipe pipe)
    {
        if (pipe.travelPointIndex+1 >= travelPoints.Count)
        {
            if (pipesOnBelt[0] == null) {
                pipesOnBelt[pipe.travelPointIndex] = null;
                pipesOnBelt[0] = pipe.transform;
                return travelPoints[0];
            }
            return null;
        }
        if (pipesOnBelt[pipe.travelPointIndex + 1] == null)
        {
            pipesOnBelt[pipe.travelPointIndex] = null;
            pipesOnBelt[pipe.travelPointIndex + 1] = pipe.transform;
            return travelPoints[pipe.travelPointIndex + 1];
        }
        return null;
    }

    //Removes pipe from belt, and empties spot.
    public void PickConveyorPipe(int travelIndex)
    {
        pipesOnBelt[travelIndex] = null;
    }

    public void SetHighlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            
        }
    }
}
