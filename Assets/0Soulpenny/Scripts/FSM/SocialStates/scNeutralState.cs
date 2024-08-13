using ECM2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public class scNeutralState : BaseState
    {
        private FPPlayerCharacter character;
        public scNeutralState(StateMachine stateMachine, FPPlayerCharacter character, AnimationSetSocial animSet) : base(stateMachine, animSet) { }


        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void Execute()
        {
            Debug.Log("Feeling neutral.");
        }


        public override IState GetNextState()
        {
            return null;
        }
    }

}