using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject textGUI;

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

        Text textGUI = GameObject.Find("WinningText").GetComponent<Text>();
        textGUI.enabled = false;
    }

    public void SpawnPlayer(GameData.Team team, GamePad.Index gamepadIndex)
    {
        //Map team to spawnlocation and instantiate player prefab
    }

    public void PlayerWon(GameData.Team winningTeam)
    {
        if (!gameRunning)
            return;
            
        Debug.Log(winningTeam.ToString() + " PLAYER WON!");
        Text textGUI = GameObject.Find("WinningText").GetComponent<Text>();
        textGUI.enabled = true;
        textGUI.GetComponent<Text>().text = winningTeam.ToString() + " PLAYER WON!";
        textGUI.GetComponent<Text>().color = GameData.TeamColors[winningTeam];
        gameRunning = false;
    }
}
