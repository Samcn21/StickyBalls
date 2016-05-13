using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostController : InputController {

    private Dictionary<int, Vector3> recordDataPlayerOne = new Dictionary<int, Vector3>();
    public bool record = false;
    public bool play = false;
    private int fixedUpdateGC;

	void Start () {
        isRecording = record;

	}

    void FixedUpdate() 
    {
        if (play) 
            FixedUpdatePlay();

        if (record) 
            FixedUpdateRecord();
    }

    private void FixedUpdateRecord()
    {
        if (index == GamepadInput.GamePad.Index.One)
        {
            recordDataPlayerOne.Add(fixedUpdateCounter, playerForce);
        }
    }

    private void FixedUpdatePlay()
    {
        fixedUpdateGC++;
        if (recordDataPlayerOne.ContainsKey(fixedUpdateGC))
        {
            if (index == GamepadInput.GamePad.Index.One)
            {
                playerForce = recordDataPlayerOne[fixedUpdateGC];
            }
        }

    }

    void Update () {
	    
	}
}
