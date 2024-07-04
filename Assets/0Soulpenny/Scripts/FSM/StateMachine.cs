using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public abstract class StateMachine: MonoBehaviour
    {
        protected IState currentState;
        protected bool isTransitioningState = false;

        protected virtual void Start()
        {
            if(currentState != null)
            {
                currentState.Enter();
            }
        }

        protected virtual void Update()
        {
            if (currentState != null) { return; }

            IState nextState = currentState.GetNextState();
            if (!isTransitioningState && nextState != currentState)
            {
                TransitionToState(nextState);
            }

        }

        public void TransitionToState(IState nextState)
        {
            if (nextState == null)
            {
                return;
            }

            isTransitioningState = true;
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
            isTransitioningState= false;
        }

    }

}
