using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] private Transform heldPipe;
    [SerializeField] private Transform heldPipeContainer;

    private MeshRenderer heldPipeRenderer;
    private PipeData.PipeType heldPipeType;
    private PipeMan pipeMan;

    void Start()
    {
        pipeMan = GameController.Instance.PipeMan;
        heldPipeRenderer = heldPipe.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        
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
    
    public void PickupPipe(ConveyorPipe conPipe)
    {
        HeldPipeType = conPipe.PipeType;
        conPipe.PickPipe();
    }

    public void PlacePipe()
    {
        HeldPipeType = PipeData.PipeType.Void;
    }
    
    public void RotatePipe(int angle)
    {
        heldPipe.rotation = Quaternion.Euler(90, angle, 0);
    }
}
