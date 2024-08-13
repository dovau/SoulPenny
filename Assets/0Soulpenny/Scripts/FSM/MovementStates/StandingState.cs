using ECM2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace Soul
{
    public class StandingState : MovementStateBase
    {
        [SerializeField] private FPPlayerCharacter character;
        [SerializeField] private FPPlayerBrain brain;
        
        public AnimationClip clip;

        public StandingState (StateMachine stateMachine, FPPlayerCharacter character, AnimationSetMovement animSet) : base(stateMachine, animSet) 
        {
            this.character = character;
        }
        public override void Enter()
        {
            
        }
        public override void Exit()
        {

        }
        public override void Execute()
        {
            Debug.Log("I'm standing still.");
            //if (character.Controls.Base.Move.triggered)
            //{
            //    stateMachine.TransitionToState(new WalkingState(stateMachine, character));
            //}

        }

        public override IState GetNextState()
        {
            //
            //

            return null;
        }

    }


}
