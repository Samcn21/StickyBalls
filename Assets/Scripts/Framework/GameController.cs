using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject playerColorAssign;

    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameController>();
            return instance;
        }
    }

    public PipeMan PipeMan;
    public GridController GridController;
    public List<Player> Players;
    public bool gameRunning { get; private set; }

    void Start()
    {
        Players = new List<Player>();

        gameRunning = true;

        playerColorAssign = GameObject.Find("WinningText");
        if (playerColorAssign != null){
            Text text = playerColorAssign.GetComponent<Text>();
            text.enabled = false;
        }
    }

    public void SpawnPlayer(GameData.Team team, GamePad.Index gamepadIndex)
    {
        //Map team to spawnlocation and instantiate player prefab
    }

    public void PlayerWon(GameData.Team winningTeam)
    {
        if (!gameRunning)
            return;

        gameRunning = false;

        if (playerColorAssign != null)
        {
            Text text = playerColorAssign.GetComponent<Text>();
            text.enabled = true;
            text.GetComponent<Text>().text = winningTeam.ToString() + " PLAYER WON!";
            text.GetComponent<Text>().color = GameData.TeamColors[winningTeam];
           
        }

    }
}
