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
            // Initialize other states similarly...
        }

        protected override void Update()
        {
            base.Update();
            Debug.Log("Move Input = "+ moveInput);
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

            jump.performed += HandleJumpInput;
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



        //Binding and subscribing on Start cause the nulls I'm getting might be that
        //OnEnable is too early for it. I'll check script execution order and learn about it
        ////but for now it's gonna be done on start
        //private void OnEnable()
        //{
        //    BindInputActions();

        //    SubscribeToInputEvents();
        //}

        private void OnDestroy()
        {
            UnSubscribeFromInputEvents();
        }
        public void HandleMovementInput(InputAction.CallbackContext context)
        {
            //moveInput = context.ReadValue<Vector2>();

            moveInput.x=context.ReadValue<Vector2>().x;
            moveInput.y=context.ReadValue<Vector2>().y;

            Debug.Log(moveInput);
            Vector3 movementDirection = Vector3.zero;
            movementDirection += _character.GetRightVector() * moveInput.x;
            movementDirection += _character.GetForwardVector() * moveInput.y;

            _character.SetMovementDirection(movementDirection);
            
            
            // Also try moveinput.normalized or moveinput.normalized.sqrMagnitude
            //Or actually learn what sqrMagnitude is properly 
            if (moveInput.sqrMagnitude > 0.1f)

            {
                TransitionToState(walkingState);
            }
            else
            {
                TransitionToState(standingState);
            }
        }
        //public void HandleMovementInput()
        //{


        //    Vector2 moveInput = new Vector2()
        //    {
        //        x = Input.GetAxisRaw("Horizontal"),
        //        y = Input.GetAxisRaw("Vertical")
        //    };

        //    Vector3 movementDirection = Vector3.zero;

        //    movementDirection += _character.GetRightVector() * moveInput.x;
        //    movementDirection += _character.GetForwardVector() * moveInput.y;

        //    _character.SetMovementDirection(movementDirection);
        //    //TransitionToState(walkingState);


        //    if (moveInput.sqrMagnitude > 0.1f)
        //    {
        //        TransitionToState(walkingState);
        //    }
        //    else
        //    {
        //        TransitionToState(standingState);
        //    }

        //}
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



        private void HandleCrouchInput(InputAction.CallbackContext context)
        {
            _character.Crouch();
        }

        //private void HandleJumpInput()
        //{
        //    _character.Jump();
        //    //if (jumpAction.triggered)
        //    //{
        //    //    _character.Jump();
        //    //}
        //    //else if (_character.IsGrounded())
        //    //{
        //    //    _character.StopJumping();
        //    //}
        //}
        private void HandleJumpInput(InputAction.CallbackContext context)
        {
            _character.Jump();
        }

        private void HandleSprintInput(InputAction.CallbackContext context)
        {
            Debug.Log("I'm sprinting!");

            // Implement sprint logic here
        }
        private void CheckForIdleState()
        {
            Vector2 inputMove = movement.ReadValue<Vector2>();

            if (inputMove.sqrMagnitude == 0 && currentState != standingState)
            {
                TransitionToState(standingState);
            }
        }
    }

}
