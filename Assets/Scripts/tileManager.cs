using UnityEngine;
using System.Collections;

public class tileManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void rotateToRight()
    {
       transform.Rotate(0, transform.rotation.eulerAngles.y + 90, 0);
    }

    public void rotateToLeft()
    {
        transform.Rotate(0, transform.rotation.eulerAngles.y - 90, 0);
    }
}
