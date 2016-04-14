using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 1f;
    public PipeData.PipeType HeldPipeType { get; private set; }
    
    public void PickupPipe(ConveyorPipe conPipe)
    {
        HeldPipeType = conPipe.PipeType;
        conPipe.PickPipe();
    }

    public void PlacePipe()
    {
        HeldPipeType = PipeData.PipeType.Void;
    }
}
