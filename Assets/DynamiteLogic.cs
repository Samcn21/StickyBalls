using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamiteLogic : MonoBehaviour {
    private HashSet<GameObject> pipeToPhysicallyAffect;

    void Awake()
    {
        pipeToPhysicallyAffect = new HashSet<GameObject>();
    }

    void OnTriggerStay(Collider col)
    {
        if (gameObject.tag == "Pipe")
        {
            pipeToPhysicallyAffect.Add(col.gameObject);
            Debug.Log("I detected the pipe " + col.gameObject.name);
        }
    }
}
