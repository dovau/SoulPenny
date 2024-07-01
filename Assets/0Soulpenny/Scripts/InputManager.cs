using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Soul
{

    public class InputManager : MonoBehaviour
    {

        private PlayerInput playerInput;
        public InputAction moveAction;
        public InputAction jumpAction;
        public InputAction vaultAction;
        public InputAction crouchAction;
        public InputAction sprintAction;
        public InputAction primaryAction;
        public InputAction secondaryAction;
        public InputAction interactAction;
        public InputAction dropAction;
        public InputAction switchAction;
        public InputAction throwAction;
        public InputAction holsterAction;
        public InputAction altAction;
        public InputAction reloadAction;


        public event Action<Vector2> OnMove;
        public event Action OnJump;
        public event Action OnVault;
        public event Action OnCrouch;
        public event Action OnSprint;
        public event Action OnPrimary;
        public event Action OnSecondary;
        public event Action OnInteract;
        public event Action OnDrop;
        public event Action OnSwitch;
        public event Action OnThrow;
        public event Action OnHolster;
        public event Action OnAlt;
        public event Action OnReload;

        //Movement

        private FPPlayerCharacter _character;
        private FPPlayerControls _controls;

        [Space(15.0f)]
        public bool invertLook = true;

        [Tooltip("Mouse look sensitivity")]
        public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);

        [Space(15.0f)]
        [Tooltip("How far in degrees can you move the camera down.")]
        public float minPitch = -80.0f;
        [Tooltip("How far in degrees can you move the camera up.")]
        public float maxPitch = 80.0f;

        private void Awake()
        {
            _character = GetComponent<FPPlayerCharacter>();
            _controls = new FPPlayerControls();
            playerInput = GetComponent<PlayerInput>();

            //Set up

            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            vaultAction = playerInput.actions["Vault"];
            crouchAction = playerInput.actions["Crouch"];
            sprintAction = playerInput.actions["Sprint"];
            primaryAction = playerInput.actions["Primary"];
            secondaryAction = playerInput.actions["Secondary"];
            interactAction = playerInput.actions["Interact"];
            dropAction = playerInput.actions["Drop"];
            throwAction = playerInput.actions["Throw"];
            switchAction = playerInput.actions["Switch"];
            holsterAction = playerInput.actions["Holster"];
            altAction = playerInput.actions["Alternative"];
            reloadAction = playerInput.actions["Reload"];



            BindActions();


            //jumpAction.ReadValue<float>();

        }

        private void BindActions()
        {


            //interactAction.performed += HandleInteractPerformed;

        }


        private void OnEnable()
        {
            _controls.Enable();
            moveAction.Enable();
            jumpAction.Enable();
            vaultAction.Enable();
            crouchAction.Enable();
            sprintAction.Enable();
            primaryAction.Enable();
            secondaryAction.Enable();
            interactAction.Enable();
            dropAction.Enable();
            switchAction.Enable();
            holsterAction.Enable();
            altAction.Enable();
            reloadAction.Enable();
        }
        private void OnDisable()
        {
            _controls.Disable();
            moveAction.Disable();
            jumpAction.Disable();
            vaultAction.Disable();
            crouchAction.Disable();
            sprintAction.Disable();
            primaryAction.Disable();
            secondaryAction.Disable();
            interactAction.Disable();
            dropAction.Disable();
            switchAction.Disable();
            holsterAction.Disable();
            altAction.Disable();
            reloadAction.Disable();

        }


        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        void Update()
        {
            Vector2 move = _controls.Base.Move.ReadValue<Vector2>();
            //Debug.Log(move);

            if (_controls.Base.Primary.triggered)
            {
                Debug.Log("Hit");
            }


            if (_controls.Base.Throw.triggered)
            {

            }
            HandleMovementInput();

            HandleMouseLook();

            HandleCrouch();
            HandleJump();


        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnInteract.Invoke();
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnJump?.Invoke();
            }
        }

        public void Vault(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnVault?.Invoke();
            }
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnCrouch?.Invoke();
            }
        }

        public void Sprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnSprint?.Invoke();
            }
        }

        public void Primary(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnPrimary?.Invoke();
            }
        }

        public void Secondary(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnSecondary?.Invoke();
            }
        }

        public void Drop(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnDrop?.Invoke();
            }
        }

        public void Switch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnSwitch?.Invoke();
            }
        }

        public void Throw(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnThrow?.Invoke();
            }
        }

        public void Holster(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnHolster?.Invoke();
            }
        }
        public void Alternative(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAlt?.Invoke();
            }
        }

        public void Reload(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnReload.Invoke();
            }
        }

        public void HandleMovementInput()
        {
            // Movement input, relative to character's view direction

            Vector2 inputMove = new Vector2()
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical")
            };

            Vector3 movementDirection = Vector3.zero;

            movementDirection += _character.GetRightVector() * inputMove.x;
            movementDirection += _character.GetForwardVector() * inputMove.y;

            _character.SetMovementDirection(movementDirection);
        }


        public void HandleMouseLook()
        {
            Vector2 lookInput = new Vector2
            {
                x = Input.GetAxisRaw("Mouse X"),
                y = Input.GetAxisRaw("Mouse Y")
            };

            lookInput *= mouseSensitivity;

            _character.AddControlYawInput(lookInput.x);
            _character.AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);
        }

        private void HandleCrouch()
        {
            if (crouchAction.triggered)
                _character.Crouch();
            if (crouchAction.WasReleasedThisFrame())
                _character.UnCrouch();
        }

        private void HandleJump()
        {
            if (jumpAction.triggered)
            {
                _character.Jump();
            }
            else if (_character.IsGrounded())
            {
                _character.StopJumping();
            }
        }
    }


}