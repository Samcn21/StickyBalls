using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;

public class MainMenu : MonoBehaviour
{
    private GamepadState gamepadState;
    private float holdTimer = 0;
    private float holdTimerMax = 0.5f;
    private int selectIndex = 0;
    private bool firstMove = false;
    
    [SerializeField] private List<MenuOption> menuOptions;
    [SerializeField] private Sprite selectedOption;
    [SerializeField] private Sprite deselectedOption;

	// Use this for initialization
	void Start ()
	{
	    holdTimer = holdTimerMax;
        menuOptions[selectIndex].Select(true, selectedOption);
	}

    void FixedUpdate()
    {
        gamepadState = GamePad.GetState(GamePad.Index.Any);
        if (gamepadState.Down || gamepadState.LeftStickAxis.y < -0.2f)
        {
            if (holdTimer <= 0 || !firstMove)
            {
                if (selectIndex < 2)
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
                    //Run FFA
                    break;
                case 1:
                    //Run 2vs2
                    break;
                case 2:
                    Application.Quit();
                    break;
            }
        }
    }

    public void UpdateSelection()
    {
        foreach (MenuOption menuOption in menuOptions)
        {
            menuOption.Select(false, deselectedOption);
        }
        menuOptions[selectIndex].Select(true, selectedOption);
    }

}
