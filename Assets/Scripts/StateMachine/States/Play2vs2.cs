using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
using GamepadInput;

namespace Assets.Scripts.States
{
    public class Play2vs2 : IStateBase
    {
        private StateManager StateManager;
        public Play2vs2(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.Play2vs2;
        }


        public void StateUpdate()
        {
            if (GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.Any) || Input.GetKeyDown(KeyCode.Escape))
            {
                StateManager.PreActiveState = GameData.GameStates.Play2vs2;
                StateManager.SwitchState(new Pause(StateManager));
            }
        }

        public void StateFixedUpdate()
        {

        }
    }
}
