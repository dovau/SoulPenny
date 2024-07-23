using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;
using Animancer;
using Animancer.Examples.AnimatorControllers.GameKit;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Soul
{

    public class SocialStateMachine : StateMachine
    {
        private FPPlayerBrain brain;

        private IState neutralState;

        private void Awake()
        {
            Debug.Log("MovementStateMachine Awake");
            brain = GetComponent<FPPlayerBrain>();

        }
        protected override void Start()
        {
            base.Start();
            InitializeStates();
            
            currentState = neutralState; // Set initial state
            currentState.Enter();
        }

        private void InitializeStates()
        {
            Debug.Log("Initializing States");
            neutralState = new scNeutralState(this, brain.Character);
        }
        private void SubscribeToInputEvents()
        {

        }
        private void UnSubscribeFromInputEvents()
        {

        }
        private void OnDestroy()
        {
            UnSubscribeFromInputEvents();
        }

        private void BindInputActions()
        {
            Debug.Log("Binding Input Actions");


        }
    }

}