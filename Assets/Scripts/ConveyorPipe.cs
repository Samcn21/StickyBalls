using UnityEngine;
using System.Collections;

public class ConveyorPipe : MonoBehaviour
{
    public PipeType PipeType = PipeType.Void;

    [SerializeField] private float moveSpeed = 0.5f;
    private MeshRenderer meshRender;
    private PipeManager pipeManager;
    private ConveyorBelt conveyorBelt;
    private Transform prevTravelTarget;
    private float moveProgress = 0;

    public Transform currentTravelTarget { get; private set; }
    public int travelPointIndex = 0;

    //For testing picking up
    [SerializeField] private bool Pick = false;

	// Update is called once per frame
	void Update ()
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
	            travelPointIndex++;
	            moveProgress = 0;
	        }
	    }
	    else
        {
            moveProgress += Time.deltaTime*moveSpeed;
	        transform.position = Vector3.Lerp(prevTravelTarget.position, currentTravelTarget.position, moveProgress);
	    }

        if (Pick) PickPipe();
	}

    //Initializes ConveyorPipe with the correct material and type.
    public void Initialize(PipeType pipeType, Transform travelTarget, ConveyorBelt belt)
    {
        PipeManager nullTest = GameObject.FindGameObjectWithTag("GameController").GetComponent<PipeManager>();
        if (nullTest != null) {
            pipeManager = nullTest;
        }

        meshRender = GetComponent<MeshRenderer>();
        PipeType = pipeType;
        conveyorBelt = belt;
        GetComponent<MeshRenderer>().material = pipeManager.pipeTextures[PipeType];
        currentTravelTarget = travelTarget;
    }

    //Should return pipe that you can place, and remove this pipe from the belt.
    public void PickPipe()
    {
        conveyorBelt.PickConveyorPipe(travelPointIndex);
        Destroy(gameObject);
    }
}
