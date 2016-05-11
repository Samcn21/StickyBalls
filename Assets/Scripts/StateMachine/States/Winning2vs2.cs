using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class Winning2vs2 : IStateBase
    {

        private StateManager StateManager;
        public Winning2vs2(StateManager managerRef)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.Winning2vs2;
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