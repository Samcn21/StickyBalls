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
            //TODO if all colors are chosen or press start / enter then goes to PlayFFA
            // StateManager.SwitchState(new PlayFFA(StateManager));
        }

        public void StateFixedUpdate()
        {

        }
    }
}