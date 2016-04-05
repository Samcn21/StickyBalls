using UnityEngine;
using System.Collections;

public class CrossPipeFactory : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
            col.gameObject.GetComponent<PlayerManager>().carryCrossPipe();
    }

}
