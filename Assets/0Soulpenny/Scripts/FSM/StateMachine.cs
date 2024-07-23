using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public abstract class StateMachine: MonoBehaviour
    {
        protected IState currentState;
        public IState CurrentState { get { return currentState; } }


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
            if (currentState == null) { return; }

            IState nextState = currentState.GetNextState();
            //if (!isTransitioningState && nextState != currentState)
            //{
            //    TransitionToState(nextState);
            //}
            if ( nextState != currentState)
            {
                TransitionToState(nextState);
            }
            currentState.Execute();
        }

        public void TransitionToState(IState nextState)
        {
            if (nextState == null)
            {
                return;
            }

            //isTransitioningState = true;
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
            //isTransitioningState= false;
        }

    }

}
