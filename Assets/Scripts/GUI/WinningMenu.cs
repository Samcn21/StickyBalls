using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinningMenu : MonoBehaviour
{

    [SerializeField] private Image winningMenu;
    [SerializeField] private List<Image> winningColors; //Blue, Cyan, Purple, Yellow
    [SerializeField] private Image playAgain;
    [SerializeField] private Image playAgainSelected;
    [SerializeField] private Image mainMenu;
    [SerializeField] private Image mainMenuSelected;

    private int selected;

    private int Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            if (selected == 0)
            {
                DisableAllImages();
                playAgainSelected.enabled = true;
                mainMenu.enabled = true;
            }
            else
            {
                DisableAllImages();
                mainMenuSelected.enabled = true;
                playAgain.enabled = true;
            }
        }
    }
    private bool buttonHeld = false;

    public void PlayerWon(GameData.Team team)
    {
        winningMenu.enabled = true;
        winningColors[(int) team].enabled = true;
        Selected = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        if (!winningMenu.enabled) return;
	    GamepadState padState = GamePad.GetState(GamePad.Index.Any);

        if (GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.Any)) {
            if (selected == 0) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else {
                SceneManager.LoadScene(0);
            }
        }

        if (padState.Up || padState.LeftStickAxis.y > 0.5f)
	    {
	        if (buttonHeld) return;
	        if (selected == 1)
	            Selected--;
	        buttonHeld = true;
	    }
        else if (padState.Down || padState.LeftStickAxis.y < -0.5f)
        {
            if (buttonHeld) return;
            if (selected == 0)
                Selected++;
            buttonHeld = true;

        }
        else 
        {
            buttonHeld = false;
        }
	}

    private void DisableAllImages()
    {
        mainMenu.enabled = mainMenuSelected.enabled = playAgain.enabled = playAgainSelected.enabled = false;
    }
}
