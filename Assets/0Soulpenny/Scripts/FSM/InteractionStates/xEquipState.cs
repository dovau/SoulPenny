using ECM2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public class xEquipState : BaseState
    {
        private FPPlayerCharacter character;
        public xEquipState(StateMachine stateMachine, FPPlayerCharacter character, AnimationSetMovement animSet) : base(stateMachine, animSet)
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
            Debug.Log($"{this.GetType().Name} + state executed .");
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