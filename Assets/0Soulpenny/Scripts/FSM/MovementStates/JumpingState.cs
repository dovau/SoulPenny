using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;

namespace Soul
{
    public class JumpingState : BaseState
    {
        private FPPlayerCharacter character;

        public JumpingState(StateMachine stateMachine, FPPlayerCharacter character, AnimationSetMovement animSet) : base(stateMachine, animSet)
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
            Debug.Log("I'm jumping.");

        }
        public override IState GetNextState()
        {
            return null;
        }
    }
}
