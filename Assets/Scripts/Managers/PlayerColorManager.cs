using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using System.Linq;

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

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        foreach (GameObject check in players)
        {
            InputController InputController;
            InputController = check.gameObject.GetComponent<InputController>();
            if (InputController.team != GameData.Team.Neutral)
            {
                playerIndexColor[InputController.index] = InputController.team;
            }
        }

        //start the game
        if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One) || Input.GetKeyDown(KeyCode.Return))
        {
            if (playerIndexColor[GamePad.Index.One] != GameData.Team.Neutral)
            {
                List<GameData.Team> colorList = new List<GameData.Team>(){
                                GameData.Team.Yellow, 
                                GameData.Team.Blue, 
                                GameData.Team.Red, 
                                GameData.Team.Black
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
                foreach (KeyValuePair<GamePad.Index, GameData.Team> player in playerIndexColor)
                {
                    Debug.Log(player.Key + " - " + player.Value);
                    PlayerPrefs.SetString(player.Key.ToString(), player.Value.ToString());
                }
                Application.LoadLevel("Level01");
            }
        }
    }

}
