using ECM2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public class StandingState : BaseState
    {
        private FPPlayerCharacter character;
        public StandingState (StateMachine stateMachine, FPPlayerCharacter character) : base(stateMachine) 
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
