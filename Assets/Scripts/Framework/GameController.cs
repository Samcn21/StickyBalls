using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public GameObject winningGUI;
    private CenterMachineSprite CenterMachineSprite;
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
    public PipeStatus PipeStatus;
    public ExplosionData ExplosionData;
    public StateManager StateManager;
    public PipeParticleSystemManager PipeParticleSystemManager;
    public ProgressBarManager ProgressBarManager;
    public Dictionary<GameData.Team, Player> Players;
    public Dictionary<GameData.Team, List<Player>> PlayersCoop;
    public Dictionary<GameData.Team, PlayerSource> PlayerSources;
    public bool gameRunning { get; private set; }
    public bool isPregame = false;
    public bool Gamemode_IsCoop = false;
    public float pickupTimer;
    private GameObject gameController;
    private AudioManager AudioManager;


    void Start()
    {
        AudioManager = GameObject.FindObjectOfType<AudioManager>();

        gameController = GameObject.FindGameObjectWithTag("GameController");
        StateManager = gameController.GetComponent<StateManager>();
        CenterMachineSprite = GameObject.FindObjectOfType<CenterMachineSprite>();
        isPregame = (SceneManager.GetActiveScene().buildIndex == 0);
        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            Players = new Dictionary<GameData.Team, Player>();
            PlayersCoop = new Dictionary<GameData.Team, List<Player>>();
            PlayerSources = new Dictionary<GameData.Team, PlayerSource>();
            PlayersCoop.Add(GameData.Team.Cyan, new List<Player>());
            PlayersCoop.Add(GameData.Team.Purple, new List<Player>());
        }

        gameRunning = true;

        if (winningGUI != null)
        {
            winningGUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA && StateManager.CurrentActiveState != GameData.GameStates.Pause)
        {
            List<Player> winningPlayers = new List<Player>();
            foreach (KeyValuePair<GameData.Team, Player> pair in Players)
            {
                if (!pair.Value.isDead)
                    winningPlayers.Add(pair.Value);
            }
            if (winningPlayers.Count == 1)
                PlayerWon(winningPlayers[0].Team);
        }
        else if (StateManager.CurrentActiveState == GameData.GameStates.ColorAssignFFA)
        {

            GameObject[] allPipesParticle = GameObject.FindGameObjectsWithTag("PipesParticles");

            foreach (GameObject pipeParticle in allPipesParticle)
            {

                Destroy(pipeParticle);
            }

        }
    }

    public void SpawnPlayer(GameData.Team team, GamePad.Index gamepadIndex)
    {
        //Map team to spawnlocation and instantiate player prefab
    }

    public void PlayerWon(GameData.Team winningTeam)
    {


        if (StateManager.CurrentActiveState != GameData.GameStates.ColorAssignFFA)
        {
            if (!gameRunning)
                return;

            GameObject[] allPipes = GameObject.FindGameObjectsWithTag("Pipe");
            GameObject[] allPipesParticle = GameObject.FindGameObjectsWithTag("PipesParticles");
            GameObject[] CMSuckingParticles = GameObject.FindGameObjectsWithTag("CMSucking");
            foreach (GameObject pipe in allPipes)
            {
                pipe.GetComponent<PipesSprite>().FindWinnerPipes(winningTeam);
            }

            foreach (GameObject pipeParticle in allPipesParticle)
            {
                Destroy(pipeParticle);
            }

            foreach (GameObject cmP in CMSuckingParticles)
            {
                Destroy(cmP);
            }

            AudioManager.PlayCenterMachineConnection();
            CenterMachineSprite.FindCentralMachineStatus(winningTeam);

            gameRunning = false;

            StartCoroutine(ShowWinnerGUI(winningTeam));
        }

    }

    public void Lose(GameData.Team team)
    {
        if (!PlayerSources.ContainsKey(team))
            return;
        if (PlayerSources[team] != null)
            PlayerSources[team].Explode();
        if (Gamemode_IsCoop)
        {
            List<Player> toLose = PlayersCoop[team];

            foreach (Player player in toLose)
            {
                player.EnableCryParticles();
                player.Die();
            }
        }
        else
        {
            Players[team].EnableCryParticles();
            Players[team].Die();
        }

    }

    IEnumerator ShowWinnerGUI(GameData.Team color)
    {
        yield return new WaitForSeconds(3);

        if (winningGUI != null)
        {
            winningGUI.gameObject.SetActive(true);

            Text text = winningGUI.GetComponentInChildren<Text>();
            text.GetComponent<Text>().text = color.ToString().ToUpper() + " PLAYER WON!";
            text.GetComponent<Text>().color = GameData.TeamColors[color];
        }
    }
}
