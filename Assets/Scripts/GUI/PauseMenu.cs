using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    private bool isPaused = false;
    private float stickSensivity = 0.25f;
    private ScreenFader ScreenFader;
    private GameObject[] buttons;
    private bool screenFader = false;
    
    private GamepadState gamepadState;
    private float holdTimer = 0;
    private float holdTimerMax = 0.5f;
    private int selectIndex = 0;
    private bool firstMove = false;
    private string levelName;
    public string mainMenu = "MainMenu";
    private GameObject tc;
    private AudioManager AudioManager;
    public float buttonPressedDelay = 0.7f;
    private float buttonPressedDelayReset; 

    [SerializeField] private List<MenuOption> menuOptions;
    [SerializeField] private List<Sprite> selectedOption;
    [SerializeField] private List<Sprite> deselectedOption;

    void Awake () {
        levelName = SceneManager.GetActiveScene().name;
        mainMenu = "MainMenu";

        holdTimer = holdTimerMax;
        menuOptions[selectIndex].Select(true, selectedOption[selectIndex]);

        screenFader = true;
        isPaused = false;

        ScreenFader = GameObject.FindGameObjectWithTag("FadeImg").GetComponent<ScreenFader>();
        if (ScreenFader == null)
        {
            Debug.LogError("The scene must have screen fader prefab. it's in Prefabs > GUI > pfbScreenFader");
        }
        buttons = GameObject.FindGameObjectsWithTag("PauseMenuButton");

        AudioManager = GameObject.FindObjectOfType<AudioManager>();

	}

	void Update () {

        if (isPaused)
        {
            GameObject tc = GameObject.FindGameObjectWithTag("TutorialCanvas");

            if (tc != null)
                GameObject.FindGameObjectWithTag("TutorialCanvas").GetComponent<CanvasGroup>().alpha = 0;

            ScreenFader.FadeToBlack();
            foreach(GameObject btn in buttons)
            {
                btn.GetComponent<Image>().enabled = true;
            }
            PauseMenuOperation();
        }
        else 
        {
            buttonPressedDelay = buttonPressedDelayReset;
            ScreenFader.FadeToClear();
            foreach (GameObject btn in buttons)
            {
                btn.GetComponent<Image>().enabled = false;
            }

            GameObject tc = GameObject.FindGameObjectWithTag("TutorialCanvas");

            if (tc != null)
                GameObject.FindGameObjectWithTag("TutorialCanvas").GetComponent<CanvasGroup>().alpha = 1;

        }
	}

    private void PauseMenuOperation() 
    {
        gamepadState = GamePad.GetState(GamePad.Index.Any);
        if (gamepadState.Down || gamepadState.LeftStickAxis.y < -0.2f)
        {
            if (holdTimer <= 0 || !firstMove)
            {
                if (selectIndex < 3)
                { 
                    selectIndex++;
                    AudioManager.PlayMenuNav(true);
                }
                firstMove = true;
            }
            else
            {
                holdTimer -= Time.deltaTime;
            }
        }
        else if (gamepadState.Up || gamepadState.LeftStickAxis.y > 0.2f)
        {
            if (holdTimer <= 0 || !firstMove)
            {
                if (selectIndex > 0)
                { 
                    selectIndex--;
                    AudioManager.PlayMenuNav(true);

                }
                firstMove = true;
            }
            else
            {
                holdTimer -= Time.deltaTime;
            }
        }
        else
        {
            holdTimer = holdTimerMax;
            firstMove = false;
        }
        UpdateSelection();

        if (gamepadState.A)
        {
            AudioManager.PlayMenuNav(false);
            switch (selectIndex)
            {
                case 0:
                    isPaused = false;
                    break;
                case 1:
                    SceneManager.LoadScene(levelName);
                    break;
                case 2:
                    SceneManager.LoadScene(mainMenu);
                    break;
                case 3:
                    Application.Quit();
                    break;
            }
        }
    }

    public void UpdateSelection()
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {
            if (i == selectIndex)
            {
                menuOptions[i].Select(true, selectedOption[i]);
            }
            else
            {
                menuOptions[i].Select(false, deselectedOption[i]);
            }
        }
    }

    public void DoPause(bool state)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (state)
        {
            foreach(GameObject player in players)
            {
                player.GetComponent<InputController>().enabled = false;
            }
            //Time.timeScale = 0;   //if I use timescale the screen fader doesn;t work so I disable input controller
            //in this case we could still have the music countining 
            isPaused = true;
            
        }
        else
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<InputController>().enabled = true;
            }
            //Time.timeScale = 1;
            isPaused = false;
        }
    }
}

