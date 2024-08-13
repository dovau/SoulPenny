using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;

namespace Soul
{
    public class xIdleState : BaseState
    {
        private FPPlayerCharacter character;

        public xIdleState (StateMachine stateMachine, FPPlayerCharacter character, AnimationSetInteraction animSet) : base(stateMachine, animSet)
        {
            this.character = character;
        }
        // Start is called before the first frame update
        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void Execute()
        {
            Debug.Log("Idle Interaction state executed .");
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