using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Interfaces
{
    public interface IStateBase {
        //for force states to have Update State
        void StateUpdate();

        //to force states to have FixedUpdate State
        void StateFixedUpdate();

        //we can do the same thing for OnGUI
        //in active gamePadIndex the gamePadIndex can use this unity functions if it needed
    }
}