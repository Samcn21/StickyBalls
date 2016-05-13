using UnityEngine;
using System.Collections;
using GamepadInput;

public class VirtualPlayer : MonoBehaviour {
    private GameObject[] players;
    public GamePad.Index index;

	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        GetComponent<CharacterSprite>().FindDanceAnimation();
	}
	
	void Update () {
        foreach (GameObject player in players) 
        {
            InputController InputController = player.GetComponent<InputController>();
            if (InputController.index == index)
            {
                GetComponent<CharacterSprite>().FindMaterialVirtualPlayer(InputController.team);
            }
            
        }
	}
}
