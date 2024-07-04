using ECM2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public interface IState
    {
        public void Enter();
        public void Execute();
        public void Exit();
        IState GetNextState();
    }
    public abstract class BaseState: IState
    {
        protected StateMachine stateMachine;

        public BaseState(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Execute();
        public abstract IState GetNextState();
    }

}
