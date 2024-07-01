using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soul
{
    public abstract class BaseState<EState> where EState : Enum
    {

        public BaseState(EState key)
        {
            StateKey = key;
        }

        public EState StateKey { get; private set;}


        public abstract void Enter();
        public abstract void Exit();
        public abstract void Execute();
        public abstract EState GetNextState();
    }

}
