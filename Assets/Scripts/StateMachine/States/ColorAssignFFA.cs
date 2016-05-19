using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class ColorAssignFFA : IStateBase
    {
        private StateManager StateManager;
        public ColorAssignFFA(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.ColorAssignFFA;
        }

        public void StateUpdate()
        {
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    StateManager.PreActiveState = GameData.GameStates.ColorAssignFFA;
            //    StateManager.SwitchState(new Pause(StateManager));
            //}
        }

        public void StateFixedUpdate()
        {

        }
    }
}