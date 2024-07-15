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

    public enum IStatePriority
    {
        //Took this from Animancer to Soul namespace

        Low,// Could specify "Low = 0," if we want to be explicit or change the order.
        Medium,// Medium = 1,
        High,// High = 2,
    }

    public abstract class BaseState: IState
    {
        protected StateMachine stateMachine;
        public List<Transition> transitions;

        public BaseState(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            //transitions = new List<Transition>();
            //transitions.Add(new Transition { TargetState = this, Condition = () => { return true; } }); //z999 idea on how to handle transitions
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Execute();
        public abstract IState GetNextState();

        public virtual IStatePriority Priority => IStatePriority.Low;


    }



    public struct Transition
    {
        public string Description;
        public Func<bool> Condition;
        public IState TargetState;
    }
}
