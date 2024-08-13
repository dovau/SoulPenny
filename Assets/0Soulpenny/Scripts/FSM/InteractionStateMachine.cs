using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Soul
{
    public class InteractionStateMachine : StateMachine
    {
        public FPPlayerBrain brain;
        public AnimationSetInteraction InitialInteractionAnimSet;
        public AnimationSetInteraction currentInteractionAnimSet;
        //private bool itemHolstered;
        public bool ItemHolstered;
        //private bool canHolster;
        public bool CanHolster;

        public HandType HandType;
        event Action localBrainInitialized;

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
        private InteractionStates initialState;
        public InteractionStates InitialState { get { return initialState; } set { initialState = value; } }

        private IState xNoneState;
        private IState xIdleState;
        private IState interactState;
        private IState holdingState;
        private IState useState;


        public void Initialize(FPPlayerBrain brain, HandType hand)
        {
            this.brain = brain;
            if (brain != null)
            {
                Debug.Log($"InteractionSM: Brain added to: {this.name}");

                //Debug.Log("SM Initialization complete");
            }
            HandType = hand;
            Debug.Log($"InteractionSM {this.name} + hand: {HandType}");



            if(brain == null)
            {
                Debug.Log("Got no brain");
                //GetBrain();

            }
            SubscribeToInputEvents();
        }

        private void Awake()
        {
            currentInteractionAnimSet = InitialInteractionAnimSet;

        }

        private void OnEnable()
        {



        }

        private void OnDisable()
        {
            
        }

        protected override void Start()
        {
            base.Start();

            InitializeStates();
            currentState = xIdleState;
            currentState.Enter();

        }


        private void InitializeStates()
        {
            Debug.Log("Initializing Interaction States");
            xNoneState = new xNoneState(this, brain.Character, currentInteractionAnimSet) ;
            xIdleState = new xIdleState(this, brain.Character, currentInteractionAnimSet) ;
        }

        //private void GetBrain()
        //{
        //        brain = GetComponent<FPPlayerBrain>();
        //        Debug.Log("Initialization failed, got brain as component");

        //}

        private void SubscribeToInputEvents()
        {
            Debug.Log("Subscribing to Input Events!");
            if(brain == null) { Debug.Log("Brain null"); return; }
            brain.PrimaryAction.performed += HandlePrimaryAction;
            brain.SecondaryAction.performed += HandleSecondaryAction;
            brain.InteractAction.performed += HandleInteractAction;

            brain.HolsterAction.performed += HandleHolsterAction;
            
            brain.PrimaryAction.Enable();
            brain.SecondaryAction.Enable();
            brain.InteractAction.Enable();
            brain.HolsterAction.Enable();

            Debug.Log("Subscribed to Input Events!");

        }

        private void UnSubscribeFromInputEvents()
        {
            if (brain == null) { return; }
            brain.PrimaryAction.performed -= HandlePrimaryAction;
            brain.SecondaryAction.performed -= HandleSecondaryAction;
            brain.InteractAction.performed -= HandleInteractAction;
            brain.HolsterAction.performed-= HandleHolsterAction;

            brain.PrimaryAction.Disable();
            brain.SecondaryAction.Disable();
            brain.InteractAction.Disable();
            brain.HolsterAction.Disable();
        }

        private void HandleHolsterAction(InputAction.CallbackContext context)
        {
            if (ItemHolstered && CanHolster)
            {
                Debug.Log("Item was holstered, unholstering / drawing");
                TransitionToState(xIdleState);
                ItemHolstered = false;
                
            }
            else if (ItemHolstered && !CanHolster)
            {
                
                Debug.Log("Can't draw / sheathe at the moment");
                return;
            }
            else if (!ItemHolstered && CanHolster)
            {
                ItemHolstered= true;
                TransitionToState(xNoneState);
                Debug.Log("Item was drawn, holstering / sheathing");

            }
            else if(!ItemHolstered && !CanHolster) 
            {
                Debug.Log("Item was unholstered, but can't holster / sheathe");
            }

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