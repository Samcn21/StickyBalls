using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
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
            //if (isWinning))
            //{
            //    StateManager.SwitchState(new Winning2vs2(StateManager));
            //      
            //}
        }

        public void StateFixedUpdate()
        {

        }
    }
}
