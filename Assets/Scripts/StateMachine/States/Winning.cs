using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.States
{
    public class Winning : IStateBase
    {

        private StateManager StateManager;
        public Winning(StateManager managerRef, GameData.Team winningTeam)
        {
            StateManager = managerRef;
            StateManager.CurrentActiveState = GameData.GameStates.Winning;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                go.GetComponent<InputController>().enabled = false;
            }
            GameObject.FindGameObjectWithTag("WinningMenu").GetComponent<WinningMenu>().PlayerWon(winningTeam);
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