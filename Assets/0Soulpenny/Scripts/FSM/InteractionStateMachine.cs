using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Soul
{
	public class InteractionStateMachine : StateMachine
	{
        private FPPlayerBrain brain;

        private InputAction primaryAction;
        private InputAction secondaryAction;
        private InputAction interactAction;
        private InputAction dropAction;
        private InputAction throwAction;
        private InputAction switchAction;
        private InputAction holsterAction;
        private InputAction alternativeAction;

        public enum InteractionStates
        {
            None,
            Idle,
            Ready,
            Holding,
            Grab,
            Push,
            Pull,
            Slap,
            Steal,
            Cast,
            Thrust,
            Swing,
            Block,
            Parry,
            
            //Let's see idk yet
        }

        private IState xIdleState;
        private IState interactState;
        private IState holdingState;
        private IState useState;

        private void Awake()
        {
            Debug.Log("InteractionStateMachine Awake");
            brain = GetComponent<FPPlayerBrain>();


        }

        protected override void Start()
        {
            base.Start();

            BindInputActions();

            SubscribeToInputEvents();

            InitializeStates();
            currentState = xIdleState;
            currentState.Enter();

        }


        private void InitializeStates()
        {
            Debug.Log("Initializing Interaction States");
            xIdleState = new xIdleState(this, brain.Character);

        }
        private void SubscribeToInputEvents()
        {
            Debug.Log("Subscribing to Input Events!");

            primaryAction.performed += HandlePrimaryAction;
            secondaryAction.performed += HandleSecondaryAction;
            interactAction.performed += HandleInteractAction;
            
            primaryAction.Enable();
            secondaryAction.Enable();
            interactAction.Enable();

            Debug.Log("Subscribed to Input Events!");

        }

        private void UnSubscribeFromInputEvents()
        {
            primaryAction.performed -= HandlePrimaryAction;
            secondaryAction.performed -= HandleSecondaryAction;
            interactAction.performed -= HandleInteractAction;



            primaryAction.Disable();
            secondaryAction.Disable();
            interactAction.Disable();
        }
        private void BindInputActions()
        {
            Debug.Log("Binding Input Actions: Interactions");
            primaryAction = brain.Controls.Base.Primary;
            secondaryAction = brain.Controls.Base.Secondary;
            interactAction = brain.Controls.Base.Interact;
            dropAction = brain.Controls.Base.Drop;
            throwAction = brain.Controls.Base.Throw;
            switchAction = brain.Controls.Base.Switch;
            holsterAction = brain.Controls.Base.Holster;
            alternativeAction = brain.Controls.Base.Alternative;
            Debug.Log("Input Actions Bound!");

        }
        private void OnDestroy()
        {
            UnSubscribeFromInputEvents();
        }

        private void HandlePrimaryAction(InputAction.CallbackContext context)
        {
            Debug.Log("Primary action performed");
        }
        private void HandleSecondaryAction(InputAction.CallbackContext context)
        {
            Debug.Log("Secondary action performed");
        }
        private void HandleInteractAction(InputAction.CallbackContext context)
        {
            Debug.Log("Interact action performed");
        }
    }

}