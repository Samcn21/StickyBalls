using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{

    public Pipe pipe { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPipe(Pipe newPipe)
    {
        pipe = newPipe;
    }
}
