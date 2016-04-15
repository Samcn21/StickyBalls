using UnityEngine;
using System.Collections;
using GamepadInput;

public class ControllerDetection : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One))
        {
            Debug.Log("Num1");
        }
        else if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Two))
        {
            Debug.Log("Num2");
            
        }
        
	}
}
