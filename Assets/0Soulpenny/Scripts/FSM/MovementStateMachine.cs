using ECM2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Soul
{
    public class MovementStateMachine : StateMachine
    {   
        //private FPPlayerCharacter _character;
        //private FPPlayerControls controls;
        private FPPlayerBrain brain;
        private FPPlayerAbilityManager abilityManager;
        private FPPlayerMediator mediator;

            //private InputAction movement;
            //private InputAction jump;
            //private InputAction crouch;
            //private InputAction crawl;
            //private InputAction sprint;
            //private InputAction climb;
        //...etc

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


        private IState standingState;
        private IState walkingState;
        private IState jumpingState;

        Vector2 moveInput;
        private bool canSprint;
        public bool CanSprint => canSprint;
        private void Awake()
        {
            Debug.Log("MovementStateMachine Awake");
            brain = GetComponent<FPPlayerBrain>();

        }

        protected override void Start()
        {
            base.Start();
            

            //BindInputActions();
            
            SubscribeToInputEvents();

            InitializeStates();


            currentState = standingState; // Set initial state
            currentState.Enter();

            GetMediator();
            GetAbilityManager();
        }


 
        //private void BindInputActions()
        //{
        //    Debug.Log("Binding Input Actions");
        //    movement = brain.Controls.Base.Move;
        //    jump = brain.Controls.Base.Jump;
        //    crouch = brain.Controls.Base.Crouch;
        //    sprint = brain.Controls.Base.Sprint;

        //}
        private void GetMediator()
        {
            mediator = brain.Mediator;
            if(mediator != null ) { Debug.Log("MovementSM Found Mediator"); }
        }
        private void GetAbilityManager()
        {
            abilityManager = mediator.AbilityManager;
            if (abilityManager != null) { Debug.Log("MovementSM Found Ability Manager"); }
        }

        private void InitializeStates()
        {
            Debug.Log("Initializing States");
            standingState = new StandingState(this, brain.Character);
            walkingState = new WalkingState(this, brain.Character);
            jumpingState = new JumpingState(this, brain.Character);
            // Initialize other states similarly...
        }

        protected override void Update()
        {
            base.Update();
            //CheckForIdleState();
            Debug.Log($"Current State: {currentState}");
            UpdateMovementDirection();

            HandleGroundCheck();

        }


        private void SubscribeToInputEvents() 
        {
            Debug.Log("Subscribing to Input Events");
            brain.Movement.performed += HandleMovementInput;
            brain.Movement.canceled += HandleMovementInput; 

            brain.Jump.started += HandleJumpInput;
            brain.Jump.performed += HandleJumpInput;
            brain.Jump.canceled += HandleJumpInput;

            brain.Crouch.performed += HandleCrouchInput;

            //sprint.started += HandleSprintInput;
            brain.Sprint.performed += HandleSprintInput;
            brain.Sprint.canceled += HandleSprintInput;

            brain.Movement.Enable();
            brain.Jump.Enable();
            brain.Crouch.Enable();
            brain.Sprint.Enable();

        }
        private void UnSubscribeFromInputEvents()
        {
            Debug.Log("Unsubscribing from Input Events");
            brain.Movement.performed -= HandleMovementInput;
            brain.Movement.canceled -= HandleMovementInput;

            brain.Jump.performed -= HandleJumpInput;
            brain.Crouch.performed -= HandleCrouchInput;

            //sprint.started -= HandleSprintInput;
            brain.Sprint.performed -= HandleSprintInput;
            brain.Sprint.canceled -= HandleSprintInput;


            brain.Movement.Disable();
            brain.Jump.Disable();
            brain.Crouch.Disable();
            brain.Sprint.Disable();
        }

        private void OnDestroy()
        {
            UnSubscribeFromInputEvents();
        }


        public void HandleMovementInput(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.sqrMagnitude > 0 && brain.Character.IsGrounded() ) 
            {
                TransitionToState(walkingState);

            }
            else if (moveInput.sqrMagnitude == 0 && brain.Character.IsGrounded() )
            {
                TransitionToState(standingState); 
            
            }
        }

        private void UpdateMovementDirection()
        {
            Vector3 movementDirection = Vector3.zero;
            movementDirection += brain.Character.GetRightVector() * moveInput.x;
            movementDirection += brain.Character.GetForwardVector() * moveInput.y;
            brain.Character.SetMovementDirection(movementDirection);
            //Debug.Log("Movement Direction: " + movementDirection);
        }

        private void HandleCrouchInput(InputAction.CallbackContext context)
        {
            brain.Character.Crouch();
        }

        private void HandleJumpInput(InputAction.CallbackContext context)
        {
            if(context.canceled) return;
            if(context.started)
            {
                Debug.Log("CTX Started Jumping");
                brain.Character.Jump();
                TransitionToState(jumpingState);
            }
            else if(context.performed)
            {
                Debug.Log("CTX Jump Performed");
                brain.Character.StopJumping();
            }

        }

        private void HandleGroundCheck()
        {
            if (currentState == jumpingState && brain.Character.IsGrounded())
            {
                //_character.StopJumping();
                TransitionToState(standingState);
                Debug.Log("Character is grounded: " + brain.Character.IsGrounded());
            }
        }
        private void HandleSprintInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Sprinting");
                canSprint = true;
                var sprintAbility = abilityManager?.GetAbility<CharacterSprint>();
                if (sprintAbility != null)
                {
                    Debug.Log("Performed & Sprint ability successfully received from Ability Manager");

                    sprintAbility.Activate();
                }
                else
                {
                    Debug.LogError("CharacterSprint ability not found");
                }

            }

            else if (context.canceled)
            {
                if (canSprint)
                {

                    Debug.Log("Not Sprinting");
                    canSprint = false;
                    var sprintAbility = abilityManager?.GetAbility<CharacterSprint>();
                    if (sprintAbility != null)
                    {
                        Debug.Log("Canceling & Sprint ability successfully received from Ability Manager");

                        sprintAbility.Deactivate();
                    }
                    else
                    {
                        Debug.LogError("CharacterSprint ability not found");
                    }
                }
            }

            //if (context.started)
            //{
            //    Debug.Log("Sprinting");
            //    canSprint = true;
            //    var sprintAbility = abilityManager?.GetAbility<CharacterSprint>();
            //    if (sprintAbility != null)
            //    {
            //        Debug.Log("Starting & Sprint ability successfully received from Ability Manager");
            //        sprintAbility.Activate();
            //    }
            //    else
            //    {
            //        Debug.LogError("CharacterSprint ability not found");
            //    }
            //}



        }
    }

}
