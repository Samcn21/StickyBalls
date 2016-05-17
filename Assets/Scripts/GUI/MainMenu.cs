using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private GamepadState gamepadState;
    private float holdTimer = 0;
    private float holdTimerMax = 0.5f;
    private int selectIndex = 0;
    private bool firstMove = false;
    
    [SerializeField] private List<MenuOption> menuOptions;
    [SerializeField] private List<Sprite> selectedOption;
    [SerializeField] private List<Sprite> deselectedOption;

    public string sceneColorAssign = "ColorAssignFFA";
    public string scene2vs2        = "Level2vs2";
    public string credits          = "Credits";


	// Use this for initialization
	void Start ()
	{
	    holdTimer = holdTimerMax;
        menuOptions[selectIndex].Select(true, selectedOption[selectIndex]);
	}

    void FixedUpdate()
    {
        gamepadState = GamePad.GetState(GamePad.Index.Any);
        if (gamepadState.Down || gamepadState.LeftStickAxis.y < -0.2f)
        {
            if (holdTimer <= 0 || !firstMove)
            {
                if (selectIndex < 3)
                    selectIndex++;
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
                    selectIndex--;
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
            switch (selectIndex)
            {
                case 0:
                    SceneManager.LoadScene(sceneColorAssign);
                    break;
                case 1:
                    SceneManager.LoadScene(scene2vs2);
                    break;
                case 2:
                    SceneManager.LoadScene(credits);
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

}
