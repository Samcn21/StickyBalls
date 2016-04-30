using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamiteLogic : MonoBehaviour {
    private HashSet<GameObject> pipeToPhysicallyAffect;
    [SerializeField]
    private float explosionForce;
    [SerializeField]
    private SphereCollider explosionRadiusCollider;
    void Awake()
    {
        pipeToPhysicallyAffect = new HashSet<GameObject>();
    }

    void OnTriggerStay(Collider col)
    {

        if (col.gameObject.tag == "Pipe")
        {
            pipeToPhysicallyAffect.Add(col.gameObject);
        }
    }

    public void Explode()
    {
        Debug.Log("I am exploding");
        foreach(GameObject g in pipeToPhysicallyAffect)
        {
            AffectWithExplosion(g);
        }
       
    }

    private void AffectWithExplosion(GameObject g)
    {
        Debug.Log("I am applying forces");
        Rigidbody body = g.GetComponent<Rigidbody>();
        body.isKinematic = false;
        g.GetComponent<Collider>().isTrigger = false;
        float d = Mathf.Abs(Vector3.Distance(transform.position, g.transform.position));
        body.AddForce((g.transform.position-transform.position)*explosionForce*d/explosionRadiusCollider.radius,ForceMode.Force);
    }
}
