using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
    {
        protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();            
        
        protected BaseState<EState> CurrentState;

        protected bool isTransitioningState = false;
        void Start()
        {
            CurrentState.Enter();

        }
        void Update()
        {
            EState nextStateKey = CurrentState.GetNextState();
            if(!isTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
            {
                CurrentState.Execute();

            }
            else if(!isTransitioningState)
            {
                TransitionToState(nextStateKey);
            }

        }

        public void TransitionToState(EState stateKey)
        {
            CurrentState.Exit();
            CurrentState = States[stateKey];
        }

        





    }

}
