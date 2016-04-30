using UnityEngine;
using System.Collections;

public class PipeCollisionDetection : MonoBehaviour {
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Pipe")
        {
            Pipe pipeConnection = col.gameObject.GetComponent<Pipe>();
            pipeConnection.DestroyPipe();
        }
    }
}
