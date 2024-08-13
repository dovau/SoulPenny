using ECM2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

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

    //public abstract class BaseState: IState
    //{
    //    protected StateMachine stateMachine;
    //    public List<Transition> transitions;
    //    protected AnimationSetBaseTest animationSet;

    //    public BaseState(StateMachine stateMachine, AnimationSetBaseTest animationSet)
    //    {
    //        this.stateMachine = stateMachine;
    //        this.animationSet = animationSet;   
    //        //transitions = new List<Transition>();
    //        //transitions.Add(new Transition {TargetState = this, Condition = () => { return true; } }); //z999 idea on how to handle transitions
    //    }


    //    public abstract void Enter();
    //    public abstract void Exit();
    //    public abstract void Execute();
    //    public abstract IState GetNextState();

    //    public virtual IStatePriority Priority => IStatePriority.Low;


    //}

    public abstract class BaseState : IState
    {
        protected StateMachine stateMachine;
        public List<Transition> transitions;
        protected AnimationSetBase animationSet;

        public BaseState(StateMachine stateMachine, AnimationSetBase animationSet)
        {
            this.stateMachine = stateMachine;
            this.animationSet = animationSet;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Execute();
        public abstract IState GetNextState();

        public virtual IStatePriority Priority => IStatePriority.Low;
    }

    // Specialized base class for movement-related states.
    public abstract class MovementStateBase : BaseState
    {
        protected AnimationSetMovement animationSetMovement;

        public MovementStateBase(StateMachine stateMachine, AnimationSetMovement animationSet) : base(stateMachine,animationSet)
        {
            this.animationSetMovement = animationSet;
        }
    }

    // Specialized base class for interaction-related states.
    public abstract class InteractionStateBase : BaseState
    {
        protected AnimationSetInteraction animationSetInteraction;

        public InteractionStateBase(StateMachine stateMachine, AnimationSetInteraction animationSet) : base(stateMachine, animationSet)
        {
            this.animationSetInteraction = animationSet;
        }
    }

    // Specialized base class for social-related states.
    public abstract class SocialStateBase : BaseState
    {
        protected AnimationSetSocial animationSetSocial;

        public SocialStateBase(StateMachine stateMachine, AnimationSetSocial animationSet) : base(stateMachine, animationSet)
        {
            this.animationSetSocial = animationSet;
        }
    }



    public struct Transition
    {
        public string Description;
        public Func<bool> Condition;
        public IState TargetState;
    }



}
