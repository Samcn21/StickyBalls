using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeGeneratorStealVersion : ConveyorBelt
{
    //To be moved to proper manager
   

    //Inspector Variables
    [SerializeField] private List<Transform> travelPoints;
    //Template for pipes being spat out on belt
    [SerializeField] private List<PipeData.PipeType> pipeQueueTemplate;
    [SerializeField] private float pipeSpawnInterval = 5;
    [SerializeField] private GameObject conveyorPipePrefab;

    private Transform[] pipesOnBelt; 
    private float _pipeSpawnIntervalRemaining = 0;
    private Queue<PipeData.PipeType> pipeQueue;
    private float counter = 0;
    private PipeMan pipeMan;

	void Start () {
        pipeQueue = new Queue<PipeData.PipeType>(pipeQueueTemplate.ToArray());
        pipesOnBelt = new Transform[travelPoints.Count];

        pipeMan = GameObject.FindGameObjectWithTag("GameController").GetComponent<PipeMan>();
    }
	
	void Update ()
	{
	    _pipeSpawnIntervalRemaining -= Time.deltaTime;
        if (_pipeSpawnIntervalRemaining <= 0 && pipesOnBelt[0] == null) 
            SpawnPipe();
	}

   
   
}
