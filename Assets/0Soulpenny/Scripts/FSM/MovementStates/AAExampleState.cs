using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;

namespace Soul
{
    public class AAExampleState : BaseState
    {
        private FPPlayerCharacter character;
        
        public AAExampleState (StateMachine stateMachine, FPPlayerCharacter character, AnimationSetMovement animSet) : base(stateMachine, animSet)
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
            Debug.Log("I'm " + this.GetType().Name + " ...ing");
        }
        public override IState GetNextState()
        {

            return null;
        }


    } 
}
