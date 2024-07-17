using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;
using UnityEngine.InputSystem;

namespace Soul
{
    public class FPPlayerBrain : MonoBehaviour
    {

        private FPPlayerCharacter _character;
        public FPPlayerCharacter Character { get { return _character; } }
        
        private FPPlayerControls controls;
        public FPPlayerControls Controls { get { return controls; } }


        private MovementStateMachine movementSM;
        public InteractionStateMachine interactSM;
        //private SocialStateMachine socialSM;

        private void Awake()
        {

            GetCharacter();

            GetStateMachines();

        }
        private void GetCharacter()
        {
            _character = GetComponentInParent<FPPlayerCharacter>();

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

        }

        private void Start()
        {
            BindControls();

        }

        private void BindControls()
        {
            controls = _character.Controls;
            //possible weak link here check it cause 
            //I'm trying to move logic to playerbrain AND change from
            //controls=new FPPlayerControls to character.controls so...

            if (controls == null)
            {
                Debug.LogError("FPPlayerControls is null!");
                this.enabled = false;
                return;
            }
            controls.Base.Enable();
        }
    }

}