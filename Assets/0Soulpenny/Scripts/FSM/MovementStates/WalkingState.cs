using ECM2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public class WalkingState : BaseState
    {
        private FPPlayerCharacter character;
        public WalkingState(StateMachine stateMachine, FPPlayerCharacter character, AnimationSetMovement animSet) : base(stateMachine, animSet) { }
        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void Execute()
        {
            //Animancer.Play("Walking")
            Debug.Log("I'm walking.");
        } 


        public override IState GetNextState()
        {
            return null;
        }


    }


}
