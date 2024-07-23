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

        //private InputAction primaryAction;
        //private InputAction secondaryAction;
        //public InputAction interactAction;
        //private InputAction dropAction;
        //private InputAction throwAction;
        //private InputAction switchAction;
        //private InputAction holsterAction;
        //private InputAction alternativeAction;

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

            //BindInputActions(); // z999 carried over to FPPlayerBrain

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

            brain.PrimaryAction.performed += HandlePrimaryAction;
            brain.SecondaryAction.performed += HandleSecondaryAction;
            brain.InteractAction.performed += HandleInteractAction;
            
            brain.PrimaryAction.Enable();
            brain.SecondaryAction.Enable();
            brain.InteractAction.Enable();

            Debug.Log("Subscribed to Input Events!");

        }

        private void UnSubscribeFromInputEvents()
        {
            brain.PrimaryAction.performed -= HandlePrimaryAction;
            brain.SecondaryAction.performed -= HandleSecondaryAction;
            brain.InteractAction.performed -= HandleInteractAction;

            brain.PrimaryAction.Disable();
            brain.SecondaryAction.Disable();
            brain.InteractAction.Disable();
        }
        //private void BindInputActions()
        //{
        //    Debug.Log("Binding Input Actions: Interactions");
        //    primaryAction = brain.Controls.Base.Primary;
        //    secondaryAction = brain.Controls.Base.Secondary;
        //    interactAction = brain.Controls.Base.Interact;
        //    dropAction = brain.Controls.Base.Drop;
        //    throwAction = brain.Controls.Base.Throw;
        //    switchAction = brain.Controls.Base.Switch;
        //    holsterAction = brain.Controls.Base.Holster;
        //    alternativeAction = brain.Controls.Base.Alternative;
        //    Debug.Log("Input Actions Bound!");

        //}
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