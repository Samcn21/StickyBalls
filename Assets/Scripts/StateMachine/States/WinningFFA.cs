using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class WinningFFA : IStateBase
    {

        private StateManager StateManager;
        public WinningFFA(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.WinningFFA;
        }

        public void StateUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StateManager.SwitchState(new Replay(StateManager));
            }
        }

        public void StateFixedUpdate()
        {

        }
    }
}