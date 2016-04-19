using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

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

        gameRunning = false;
    }
}
