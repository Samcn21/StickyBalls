using UnityEngine;
using System.Collections;
using GamepadInput;
using System.Collections.Generic;

public class ColorAssign : MonoBehaviour {
    public Renderer myColor;
    public Material playerNeutral;
    public GameData.Team color;
    public GamePad.Index controllerIndex;
    public bool colorPicked = false;
    public Dictionary<GamePad.Index, GameData.Team> playerColorAssign = new Dictionary<GamePad.Index, GameData.Team>();

    
    void Start () {
        myColor = GetComponent<Renderer>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("WTF! how do you expect without any player pick a color??");
        }
	}

    void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Player"))
        { 
            InputController InputController = other.gameObject.GetComponent<InputController>();
            InputController.colorPickPermit = true;
            colorPicked = InputController.colorPicked;

            if (InputController.team == GameData.Team.Neutral && colorPicked && controllerIndex == GamePad.Index.Any)
            {
                InputController.team = color;
                InputController.colorPickPermit = false;

                //TODO change color system to sprite animation
                controllerIndex = InputController.index;
                other.gameObject.GetComponentInChildren<Renderer>().material = myColor.material;
                
                playerColorAssign.Add(controllerIndex,color);
                colorPicked = false;
            }
            else if (InputController.team == GameData.Team.Neutral && controllerIndex == GamePad.Index.Any) {
                other.gameObject.GetComponentInChildren<Renderer>().material = myColor.material;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (other.gameObject.tag == "Player")
        {
            InputController InputController = other.gameObject.GetComponent<InputController>();
            InputController.colorPickPermit = false;

            if (InputController.team == GameData.Team.Neutral && controllerIndex == GamePad.Index.Any)
            {
                other.gameObject.GetComponentInChildren<Renderer>().material = playerNeutral;
            }
        }
    }
}
