using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class Replay : IStateBase
    {
        private StateManager StateManager;
        public Replay(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.Replay;
        }

        public void StateUpdate()
        {
            //TODO: when playerX pressed X button goes to restart mode, now is space on keyboard
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StateManager.SwitchState(new Restart(StateManager));
            }
        }

        public void StateFixedUpdate()
        {

        }
    }
}