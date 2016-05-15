using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class BeginState : IStateBase
    {
        private StateManager StateManager;

        //this method is constructive and works like Start() in unity method
        public BeginState(StateManager managerRef)
        {
            StateManager = managerRef;
        }

        public void StateUpdate()
        {
            switch (StateManager.CurrentActiveState)
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
                case GameData.GameStates.ColorAssign2vs2:
                    StateManager.SwitchState(new ColorAssign2vs2(StateManager));
                    break;
            }
        }

        public void StateFixedUpdate() 
        {
            //This gamePadIndex has nothing to do with fixed update  method. It's a setup gamePadIndex
        }

    }
}