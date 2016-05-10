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
    private InputController InputController;

    void Start()
    {
        pipeMan = GameController.Instance.PipeMan;
        heldPipeRenderer = heldPipe.GetComponent<MeshRenderer>();
        InputController = GetComponent<InputController>();
        heldPipe.parent = null;
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
        moveSpeed = moveSpeed/2;
    }

    public void Initialize()
    {
        GameController.Instance.Players.Add(InputController.team, this);
    }
}
