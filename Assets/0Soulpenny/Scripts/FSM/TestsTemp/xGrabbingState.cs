using UnityEngine;
using ECM2;
using Animancer;

namespace Soul
{
    public class xGrabbingState : BaseState
    {
        private FPPlayerCharacter character;

        public xGrabbingState(StateMachine stateMachine, FPPlayerCharacter character, AnimationSetSocial animSet) 
            : base(stateMachine, animSet)
        {
            this.character = character;
        }

        public override void Enter()
        {
            Debug.Log("xGrabbingState state entered.");
        }

        public override void Exit()
        {
            Debug.Log("xGrabbingState state exited.");
        }

        public override void Execute()
        {
            Debug.Log("xGrabbingState state executed.");
        }

        public override IState GetNextState()
        {
            // Define transition logic here
            return null;
        }
    }
}