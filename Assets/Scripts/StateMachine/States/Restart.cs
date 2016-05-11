using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class Restart : IStateBase
    {
        private StateManager StateManager;
        public Restart(StateManager managerRef)
        {
            StateManager = managerRef;
            //Debug.Log("Constructing Replay State");
        }

        public void StateUpdate()
        {
            StateManager.CurrentActiveState = GameData.GameStates.Restart;
            //TODO:
            //load this scene again in coroutine after couple of seconds 
            
        }

        public void StateFixedUpdate()
        {

        }
    }
}