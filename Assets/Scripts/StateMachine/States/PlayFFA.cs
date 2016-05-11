using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
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
            //TODO after this state must switch to winningFFA state
            //StateManager.SwitchState(new winningFFA(StateManager));
        }

        public void StateFixedUpdate()
        {

        }
    }
}
