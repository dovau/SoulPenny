    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;


namespace Soul
{
    public class xNoneState : BaseState
    {
        private FPPlayerCharacter character;

        public xNoneState(StateMachine stateMachine, FPPlayerCharacter character, AnimationSetInteraction animSet) : base(stateMachine, animSet)
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
            Debug.Log("None Interaction state executed .");
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