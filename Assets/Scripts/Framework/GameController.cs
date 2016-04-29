using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject winningGUI;

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
    public bool isPregame = false;

    void Start()
    {
        isPregame = (SceneManager.GetActiveScene().buildIndex == 0);
        Players = new List<Player>();

        gameRunning = true;

        winningGUI = GameObject.Find("WinningText");
        if (winningGUI != null)
        {
            Text text = winningGUI.GetComponent<Text>();
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

        if (winningGUI != null)
        {
            Text text = winningGUI.GetComponent<Text>();
            text.enabled = true;
            text.GetComponent<Text>().text = winningTeam.ToString() + " PLAYER WON!";
            text.GetComponent<Text>().color = GameData.TeamColors[winningTeam];
           
        }

    }
}
