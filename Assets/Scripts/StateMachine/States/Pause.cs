using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
using GamepadInput;
using UnityEngine.UI;

namespace Assets.Scripts.States
{
    public class Pause : IStateBase
    {
        private StateManager StateManager;
        private PauseMenu PauseMenu;
        private GameObject pauseMenu;

        public Pause(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.Pause;
            pauseMenu = GameObject.Find("PauseMenu");

            if (pauseMenu == null) 
            {
                Debug.LogError("The scene must have a pause menu canvas");
            }
            pauseMenu.GetComponent<PauseMenu>().DoPause(true);
        }

        public void StateUpdate()
        {
            if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.GetComponent<PauseMenu>().DoPause(false);

                switch(StateManager.PreActiveState)
                {
                    case GameData.GameStates.PlayFFA:
                        StateManager.SwitchState(new PlayFFA(StateManager));
                        break;

                    case GameData.GameStates.Play2vs2:
                        StateManager.SwitchState(new Play2vs2(StateManager));
                        break;

                    case GameData.GameStates.ColorAssignFFA:
                        StateManager.SwitchState(new ColorAssignFFA(StateManager));
                        break;
                }
            }
        }

        public void StateFixedUpdate()
        {

        }
    }
}