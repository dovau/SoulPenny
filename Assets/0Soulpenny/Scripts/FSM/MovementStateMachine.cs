using System;
using System.Collections;
using System.Collections.Generic;
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
        private void Awake()
        {
            Debug.Log("MovementStateMachine Awake");
            _character = GetComponent<FPPlayerCharacter>();
            if (_character == null)
            {
                Debug.LogError("FPPlayerCharacter component missing!");
                this.enabled = false;
                return;
            }

        }

        protected override void Start()
        {
            base.Start();
            controls = _character.Controls;
            if (controls == null)
            {
                Debug.LogError("FPPlayerControls is null!");
                this.enabled = false;
                return;
            }

            BindInputActions();
            InitializeStates();

            SubscribeToInputEvents();

            currentState = standingState; // Set initial state
            currentState.Enter();
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
            // Initialize other states similarly...
        }

        protected override void Update()
        {
            base.Update();
            CheckForIdleState();
            Debug.Log($"Current State: {currentState}");

            //HandleMouseLook();
            //HandleCrouch();
            //HandleJump();
        }

        private void SubscribeToInputEvents()
        {
            Debug.Log("Subscribing to Input Events");
            movement.performed += HandleMovementInput;
            movement.canceled += HandleMovementInput;

        }

        public void HandleMovementInput(InputAction.CallbackContext context)
        {
            

            Vector2 inputMove = new Vector2()
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical")
            };

            Vector3 movementDirection = Vector3.zero;

            movementDirection += _character.GetRightVector() * inputMove.x;
            movementDirection += _character.GetForwardVector() * inputMove.y;

            _character.SetMovementDirection(movementDirection);
            TransitionToState(walkingState);



        }
        //public void HandleMovementInput()
        //{
        //    // Movement input, relative to character's view direction

        //    Vector2 inputMove = new Vector2()
        //    {
        //        x = Input.GetAxisRaw("Horizontal"),
        //        y = Input.GetAxisRaw("Vertical")
        //    };

        //    Vector3 movementDirection = Vector3.zero;

        //    movementDirection += _character.GetRightVector() * inputMove.x;
        //    movementDirection += _character.GetForwardVector() * inputMove.y;

        //    _character.SetMovementDirection(movementDirection);
        //}
        
        private void CheckForIdleState()
        {
            Vector2 inputMove = movement.ReadValue<Vector2>();

            if (inputMove.sqrMagnitude == 0 && currentState != standingState)
            {
                TransitionToState(standingState);
            }
        }

        private void HandleCrouch()
        {
            _character.Crouch();
            
            // old code from when it was in fpinput
            //    if (crouchAction.triggered)
            //        _character.Crouch();
            //    if (crouchAction.WasReleasedThisFrame())
            //        _character.UnCrouch();

        }

        private void HandleJump()
        {
            _character.Jump();
            //if (jumpAction.triggered)
            //{
            //    _character.Jump();
            //}
            //else if (_character.IsGrounded())
            //{
            //    _character.StopJumping();
            //}
        }

    }

}
