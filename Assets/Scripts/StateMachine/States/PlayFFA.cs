using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
using GamepadInput;

namespace Assets.Scripts.States
{
    public class PlayFFA : IStateBase
    {
        private StateManager StateManager;
        public PlayFFA(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.PlayFFA;
        }

        public void StateUpdate()
        {
            if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Escape))
            {
                StateManager.PreActiveState = GameData.GameStates.PlayFFA;
                StateManager.SwitchState(new Pause(StateManager));
            }
        }

        public void StateFixedUpdate()
        {

        }
    }
}
