﻿using UnityEngine;
using System.Collections;
using GamepadInput;
using System.Collections.Generic;

public class ColorAssign : MonoBehaviour {
    public Renderer myColor;
    public Material playerNeutral;
    public GameData.Team team;
    private bool colorPicked = false;
    public Dictionary<GamePad.Index, GameData.Team> playerColorAssign = new Dictionary<GamePad.Index, GameData.Team>();
    
    void Start () {
        myColor = GetComponent<Renderer>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("WTF! how do you expect without any player pick a color??");
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player"))
        { 
            InputController InputController = other.gameObject.GetComponent<InputController>();
            InputController.colorPickPermit = true;

            colorPicked = InputController.colorPicked;
            if (colorPicked == false)
            {
                other.gameObject.GetComponentInChildren<Renderer>().material = myColor.material;
                playerColorAssign.Add(InputController.index, InputController.team);
                Debug.Log(playerColorAssign[0]);

            }
        }
            
    }

    void OnTriggerExit(Collider other) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (other.gameObject.tag == "Player")
        {
            InputController InputController = other.gameObject.GetComponent<InputController>();
            InputController.colorPickPermit = false;
            
            colorPicked = InputController.colorPicked;

            if (colorPicked == false)
            {
                other.gameObject.GetComponentInChildren<Renderer>().material = playerNeutral;
            }
        }
    }
}
