using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class ColorAssign2vs2 : IStateBase
    {
        private StateManager StateManager;
        public ColorAssign2vs2(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.ColorAssign2vs2;
        }

        public void StateUpdate()
        {
            //TODO if all colors are chosen or press start / enter then goes to Play2vs2
            // StateManager.SwitchState(new Play2vs2(StateManager));

        }

        public void StateFixedUpdate()
        {

        }
    }
}