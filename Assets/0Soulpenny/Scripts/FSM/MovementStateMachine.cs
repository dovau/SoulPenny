using Soul;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateMachine : StateMachine<MovementStateMachine.MovementState>
{
    public enum MovementState
    {
        Standing,
        Walking,
        Running,
        Sprinting,
        Crouching,
        Crawling,
        Jumping,
        Falling,
        Sliding,
        Rolling,
        Climbing,
        Hanging,
        Vaulting,
        Dodging
    }
    private void Awake()
    {
        CurrentState = States[MovementState.Standing];
    }
}
