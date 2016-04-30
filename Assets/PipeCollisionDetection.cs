using UnityEngine;
using System.Collections;

public class PipeCollisionDetection : MonoBehaviour {
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Pipe")
        {
            col.gameObject.GetComponent<Pipe>().DestroyPipe();
            GetComponent<Rigidbody>().isKinematic = false;
            col.gameObject.GetComponent<Rigidbody>().isKinematic = false;   
        }
    }
}
