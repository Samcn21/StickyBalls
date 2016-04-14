using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
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

    //Spawns pipe on belt on the first spot, and initializes based on which pipe in the queue it is.
    //If queue is empty, it loops it.
    private void SpawnPipe()
    {
        _pipeSpawnIntervalRemaining = pipeSpawnInterval;
        GameObject newPipe = Instantiate(conveyorPipePrefab, travelPoints[0].position, Quaternion.Euler(90, 0, 0)) as GameObject;
        newPipe.GetComponent<ConveyorPipe>().Initialize(pipeQueue.Dequeue(), travelPoints[0], this);
        if (pipeQueue.Count == 0) pipeQueue = new Queue<PipeData.PipeType>(pipeQueueTemplate.ToArray());
        newPipe.name = counter.ToString();
        counter++;
        pipesOnBelt[0] = newPipe.transform;
    }

    //Returns the next travelpoint on the belt, if the position is empty, else null.
    public Transform GetNextTravelPoint(ConveyorPipe pipe)
    {
        if (pipe.currentTravelTarget == travelPoints[travelPoints.Count - 1])
            return null;
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
