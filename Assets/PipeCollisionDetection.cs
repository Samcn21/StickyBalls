using UnityEngine;
using System.Collections;

public class PipeCollisionDetection : MonoBehaviour {
    private Rigidbody rigidBody;
    private Collider collider;
    [SerializeField]
    private float speedTreshold;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

	void Update()
    {
        if (rigidBody.velocity.magnitude > speedTreshold * speedTreshold)
            EnablePhysics();
        else
            DisablePhysics();
    }

    private void DisablePhysics()
    {
       // Debug.Log("I am disabling physics");
        rigidBody.isKinematic = true;
        collider.isTrigger = true;
    }
    private void EnablePhysics()
    {
        //Debug.Log("I am enabling physics");
        rigidBody.isKinematic = false;
        collider.isTrigger = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Pipe")
        {
            Debug.Log("Detected collision!");
            if(!col.gameObject.GetComponent<Rigidbody>().isKinematic)
            EnablePhysics();
        }
    }
}
