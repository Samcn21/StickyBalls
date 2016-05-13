﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerColorManager : MonoBehaviour
{
    public Dictionary<GamePad.Index, GameData.Team> playerIndexColor = new Dictionary<GamePad.Index, GameData.Team>() 
    {
        {GamePad.Index.One, GameData.Team.Neutral},
        {GamePad.Index.Two, GameData.Team.Neutral},
        {GamePad.Index.Three, GameData.Team.Neutral},
        {GamePad.Index.Four, GameData.Team.Neutral},
    };
    public GameObject[] players;
    private InputController InputController;
    int counter = 0;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        counter = 0;
        foreach (GameObject player in players)
        {
            InputController = player.gameObject.GetComponent<InputController>();
            if (InputController.team != GameData.Team.Neutral)
            {
                playerIndexColor[InputController.index] = InputController.team;
                counter++;
            }
        }

        //if all characters choose a color level automatically restart
        if (counter == 4) 
        {
            RestartLevel();
        }
        
        //this part starts the game from PlayerColorAssign level!
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One) || Input.GetKeyDown(KeyCode.Return))
        {
            if (playerIndexColor[GamePad.Index.One] != GameData.Team.Neutral)
            {
                List<GameData.Team> colorList = new List<GameData.Team>(){
                                GameData.Team.Yellow, 
                                GameData.Team.Blue, 
                                GameData.Team.Purple, 
                                GameData.Team.Cyan
                        };
                List<GameData.Team> impossibleColorList = new List<GameData.Team>();

                GamePad.Index[] indexList = new GamePad.Index[] {
                                GamePad.Index.One,
                                GamePad.Index.Two,
                                GamePad.Index.Three,
                                GamePad.Index.Four
                        };

                //make a list of colors that alredy picked by the player(s)
                for (int index = 0; index < indexList.Length; index++)
                {
                    foreach (GameData.Team color in colorList)
                    {
                        if (playerIndexColor[indexList[index]] == color)
                        {
                            impossibleColorList.Add(color);
                        }
                    }
                }

                //make a list of available colors that hav't picked by the player(s)
                List<GameData.Team> possibleColorList = colorList.Except(impossibleColorList).ToList();

                //assign the available colors to the players without color
                foreach (GameData.Team possibleColor in possibleColorList)
                {
                    bool oneShot = true;
                    for (int index = 0; index < indexList.Length; index++)
                    {
                        if (playerIndexColor[indexList[index]] == GameData.Team.Neutral && oneShot)
                        {
                            playerIndexColor[indexList[index]] = possibleColor;
                            oneShot = false;
                        }
                    }
                }
                RestartLevel();
            }
        }
    }

    private void RestartLevel() 
    {
        foreach (KeyValuePair<GamePad.Index, GameData.Team> player in playerIndexColor)
        {
            //Debug.Log(player.Key + " - " + player.Value);
            //TODO: Show which game pad picked what color in GUI
            PlayerPrefs.SetString(player.Key.ToString(), player.Value.ToString());
            PlayerPrefs.SetInt("isDataSaved", 1);
        }
        //TODO: in a coroutine level starts in x serconds
        //SceneManager.LoadScene("LevelFFA");
    }
}
