using UnityEngine;
using System.Collections;

public class PushMechanic : MonoBehaviour {
    [SerializeField]
    private float pushForce;
    [SerializeField]
    private bool stealMechanicActive;
    private Rigidbody body;
    private Player playerRef;
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        playerRef = GetComponent<Player>();
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            Rigidbody opponentBody = col.gameObject.GetComponent<Rigidbody>();
            if (opponentBody.velocity.normalized.magnitude >= body.velocity.normalized.magnitude)
            {
                Vector3 direction = (transform.position - col.contacts[0].point);
                direction.y = 0;
                body.AddForce( direction* pushForce, ForceMode.Impulse);
            }
            else
            {
                Vector3 direction = (col.transform.position - col.contacts[0].point);
                direction.y = 0;
                opponentBody.AddForce(direction * pushForce, ForceMode.Impulse);
                if (stealMechanicActive)
                    stealCarryingPipe(col.gameObject);
            }
            
        }
    }

    private void stealCarryingPipe(GameObject opponent)
    {
        ConveyorPipe voidPipe = new ConveyorPipe();
        ConveyorPipe heldPipe = new ConveyorPipe();
        Player opponentPlayerRef = opponent.GetComponent<Player>();
        heldPipe.PipeType = opponentPlayerRef.HeldPipeType;
        voidPipe.PipeType = PipeData.PipeType.Void;
        playerRef.PickupPipe(heldPipe);
        opponentPlayerRef.PickupPipe(voidPipe);
    }
}
