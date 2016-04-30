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
        foreach(GameObject g in pipeToPhysicallyAffect)
        {
            AffectWithExplosion(g);
            g.GetComponent<Pipe>().DestroyPipe();
        }
        Destroy(gameObject);
    }

    private void AffectWithExplosion(GameObject g)
    {
        Rigidbody body = g.GetComponent<Rigidbody>();
        body.isKinematic = false;
        g.GetComponent<Collider>().isTrigger = false;
        float d = Mathf.Abs(Vector3.Distance(transform.position, g.transform.position));
        Vector3 direction = g.transform.position - transform.position;
        direction.y = 0;
        body.AddForce(direction*explosionForce*d/(explosionRadiusCollider.radius*transform.localScale.x),ForceMode.Force);
    }
}
