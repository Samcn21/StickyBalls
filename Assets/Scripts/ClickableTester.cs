using UnityEngine;
using System.Collections;

public class ClickableTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            GetComponent<PipeManager>().placePipeOfTypeAt(PipeType.Corner, 0, 0);
	}
}
