using UnityEngine;
using ECM2;
using Animancer;

namespace Soul
{
    public class scTalkingState : BaseState
    {
        private FPPlayerCharacter character;

        public scTalkingState(StateMachine stateMachine, FPPlayerCharacter character, AnimationSetSocial animSet) 
            : base(stateMachine, animSet)
        {
            this.character = character;
        }

        public override void Enter()
        {
            Debug.Log("scTalkingState state entered.");
        }

        public override void Exit()
        {
            Debug.Log("scTalkingState state exited.");
        }

        public override void Execute()
        {
            Debug.Log("scTalkingState state executed.");
        }

        public override IState GetNextState()
        {
            // Define transition logic here
            return null;
        }
    }
}