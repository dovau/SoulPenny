using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;
using UnityEngine.InputSystem;
using Animancer;
using TMPro;
using Unity.VisualScripting;
using System;

namespace Soul
{
    public class FPPlayerBrain : CharacterBrain
    {
        private FPPlayerMediator _mediator;
        public FPPlayerMediator Mediator => _mediator;


        private FPPlayerCharacter _character;
        public FPPlayerCharacter Character { get { return _character; } }
        
        private FPPlayerControls controls;
        public FPPlayerControls Controls { get { return controls; } }


        private MovementStateMachine movementSM;
        public MovementStateMachine MovementSM => movementSM;

        private InteractionStateMachine interactSM;
        public InteractionStateMachine InteractionSM => interactSM;

        private SocialStateMachine socialSM;
        public SocialStateMachine SocialSM => socialSM;


        // Setting up Animancer stuff here for now, I'll move them to the Animation Manager or sth later

        [SerializeField]private AnimancerComponent animancer;
        [HideInInspector]public AnimancerComponent Animancer => animancer;

        /// <summary>
        /// Testing to see if I need to wait for brain to initialize for other scripts
        /// </summary>
        public event Action OnBrainInitialized;


        private InputAction movement;
        private InputAction jump;
        private InputAction crouch;
        private InputAction crawl;
        private InputAction sprint;
        private InputAction climb;

        private InputAction primaryAction;
        private InputAction secondaryAction;
        private InputAction interactAction;
        private InputAction dropAction;
        private InputAction throwAction;
        private InputAction switchAction;
        private InputAction holsterAction;
        private InputAction altModifier;
        private InputAction interactAltAction;

        // Public accessors of InputActions

        public InputAction Movement { get { return movement; } }
        public InputAction Jump { get { return jump; } }
        public InputAction Crouch { get { return crouch; } }
        public InputAction Crawl { get { return crawl; } }
        public InputAction Sprint { get { return sprint; } }
        public InputAction Climb { get { return climb; } }


        public InputAction PrimaryAction { get { return primaryAction; } }
        public InputAction SecondaryAction { get { return secondaryAction; } }
        public InputAction InteractAction { get { return interactAction; } }
        public InputAction DropAction { get { return dropAction; } }
        public InputAction AltModifier { get { return altModifier; } }

        public InputAction InteractAltAction { get { return interactAltAction; } }

        // etc testing centralizing input actions so other systems can just subscribe to Brain





        protected override void Awake()
        {
            base.Awake();

            GetStateMachines();

        }

        public void SetMediator(FPPlayerMediator mediator)
        {
            this._mediator = mediator;
        }
        public void MediatorNotifyTest()
        {
            _mediator.Notify(this, "BrainThinkingHmm");

        }

        private void GetCharacter()
        {
            //_character = GetComponentInParent<FPPlayerCharacter>();
            _character = Mediator.Character;
            if (_character == null)
            {
                Debug.LogError("FPPlayerCharacter component missing!");
                this.enabled = false;
                return;

            }
        }

        private void GetStateMachines()
        {
            movementSM = GetComponent<MovementStateMachine>();
            if (movementSM != null)
            {
                Debug.Log("Movement State Machine found");
            }
            interactSM = GetComponent<InteractionStateMachine>();
            if (interactSM != null)
            {
                Debug.Log("Interaction State Machine found");
            }
            socialSM = GetComponent<SocialStateMachine>();
            if (socialSM != null)
            {
                Debug.Log("Social State Machine found");
            }

        }

        protected override void Start()
        {
            base.Start();
            GetCharacter();
            BindControls();
            BindInputActions();
            MediatorNotifyTest();

            OnBrainInitialized?.Invoke();

        }


        private void BindInputActions()
        {
            Debug.Log("Binding Movement Input Actions");
            movement = controls.Base.Move;
            jump = controls.Base.Jump;
            crouch = controls.Base.Crouch;
            sprint = controls.Base.Sprint;
            Debug.Log("Movement Actions Bound!");
            Debug.Log("Binding Input Actions: Interactions");
            primaryAction = controls.Base.Primary;
            secondaryAction = controls.Base.Secondary;
            interactAction = controls.Base.Interact;
            dropAction = controls.Base.Drop;
            throwAction = controls.Base.Throw;
            switchAction = controls.Base.Switch;
            holsterAction = controls.Base.Holster;
            altModifier = controls.Base.Alternative;

            Debug.Log("Interaction Actions Bound!");

        }

        private void BindControls()
        {
            controls = _character.Controls;
            //possible weak link here 

            if (controls == null)
            {
                Debug.LogError("FPPlayerControls is null!");
                this.enabled = false;
                return;
            }
            controls.Base.Enable();
        }


        protected override void Update()
        {
            base.Update();



            if (Input.GetKeyDown(KeyCode.J)) 
            {
                MediatorNotifyTest();
            }
        }


   



    }

}