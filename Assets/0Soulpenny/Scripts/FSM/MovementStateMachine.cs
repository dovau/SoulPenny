using ECM2;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Soul
{
    public class MovementStateMachine : StateMachine
    {

        private FPPlayerCharacter _character;
        private FPPlayerControls controls;
        private InputAction movement;
        private InputAction jump;
        private InputAction crouch;
        private InputAction crawl;
        private InputAction sprint;
        private InputAction climb;
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

        private void Awake()
        {
            Debug.Log("MovementStateMachine Awake");
            GetCharacter();
            

        }

        protected override void Start()
        {
            base.Start();

            BindControls();

            BindInputActions();
            
            SubscribeToInputEvents();

            InitializeStates();


            currentState = standingState; // Set initial state
            currentState.Enter();
        }
        private void GetCharacter()
        {
            _character = GetComponent<FPPlayerCharacter>();

            if (_character == null)
            {
                Debug.LogError("FPPlayerCharacter component missing!");
                this.enabled = false;
                return;

            }
        }
        private void BindControls()
        {
            controls = new FPPlayerControls();

            if (controls == null)
            {
                Debug.LogError("FPPlayerControls is null!");
                this.enabled = false;
                return;
            }
            controls.Base.Enable();
        }
 
        private void BindInputActions()
        {
            Debug.Log("Binding Input Actions");
            movement = controls.Base.Move;
            jump = controls.Base.Jump;
            crouch = controls.Base.Crouch;
            sprint = controls.Base.Sprint;

        }


        private void InitializeStates()
        {
            Debug.Log("Initializing States");
            standingState = new StandingState(this, _character);
            walkingState = new WalkingState(this, _character);
            jumpingState = new JumpingState(this, _character);
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
            movement.performed += HandleMovementInput;
            movement.canceled += HandleMovementInput; 

            jump.started += HandleJumpInput;
            jump.performed += HandleJumpInput;
            jump.canceled += HandleJumpInput;

            crouch.performed += HandleCrouchInput;
            sprint.performed += HandleSprintInput;

            movement.Enable();
            jump.Enable();
            crouch.Enable();
            sprint.Enable();

        }
        private void UnSubscribeFromInputEvents()
        {
            Debug.Log("Unsubscribing from Input Events");
            movement.performed -= HandleMovementInput;
            movement.canceled -= HandleMovementInput;

            jump.performed -= HandleJumpInput;
            crouch.performed -= HandleCrouchInput;
            sprint.performed -= HandleSprintInput;


            movement.Disable();
            jump.Disable();
            crouch.Disable();
            sprint.Disable();
        }

        private void OnDestroy()
        {
            UnSubscribeFromInputEvents();
        }


        public void HandleMovementInput(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.sqrMagnitude > 0 && _character.IsGrounded() ) 
            {
                TransitionToState(walkingState);

            }
            else if (moveInput.sqrMagnitude == 0 && _character.IsGrounded() )
            {
                TransitionToState(standingState); 
            
            }
        }

        private void UpdateMovementDirection()
        {
            Vector3 movementDirection = Vector3.zero;
            movementDirection += _character.GetRightVector() * moveInput.x;
            movementDirection += _character.GetForwardVector() * moveInput.y;
            _character.SetMovementDirection(movementDirection);
            //Debug.Log("Movement Direction: " + movementDirection);
        }

        private void HandleCrouchInput(InputAction.CallbackContext context)
        {
            _character.Crouch();
        }

        private void HandleJumpInput(InputAction.CallbackContext context)
        {
            if(context.canceled) return;
            if(context.started)
            {
                Debug.Log("CTX Started Jumping");
                _character.Jump();
                TransitionToState(jumpingState);
            }
            else if(context.performed)
            {
                Debug.Log("CTX Jump Performed");
                _character.StopJumping();
            }

        }

        private void HandleGroundCheck()
        {
            if (currentState == jumpingState && _character.IsGrounded())
            {
                //_character.StopJumping();
                TransitionToState(standingState);
                Debug.Log("Character is grounded: " + _character.IsGrounded());
            }
        }
        private void HandleSprintInput(InputAction.CallbackContext context)
        {
            Debug.Log("I'm sprinting!");

            // Implement sprint logic here
        }
        //private void CheckForIdleState()
        //{
        //    if(currentState == jumpingState && _character.IsGrounded() )
        //    {
        //        Debug.Log("I'm grounded");
        //    }
        //    Vector2 inputMove = movement.ReadValue<Vector2>();

        //    if (inputMove.sqrMagnitude == 0 && currentState != standingState)
        //    {
        //        TransitionToState(standingState);
        //    }
        //}
    }

}
