using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;
using UnityEngine.InputSystem;

namespace Soul
{
    // To do:
    // Highlight to indicate an object is interactable
    // Find propixelizer shader and make outline visible
    // When ray not on the object, back to normal
    // Note for future: Could also be used as Hotspot mechanic - objects may have "Triggers"
    // or some other colliders bigger than their mesh, like a zone.
    // A collider that won't interact with physics other than raycast.
    // When Interact button is pressed, do the "Interact" based on context.
    // push, pull, hit, knock, open, use, pickup, activate (object, not the method)...etc 

    public class CharacterInteract : PlayerAbility
    {
        private bool rayActive;
        public bool RayActive => rayActive;
        Transform interactorSource;
        private float interactionRange = 5f;
        public float InteractionRange => interactionRange;

        private IInteractable currentInteractable;
        private IInteractable lastInteractable;

        [SerializeField] private bool canInteract = true; // temporary(probably) shortly to replace CanInteract()
        //CharacterHandleObject characterHandleObject; //inbound

        public override void Initialize(FPPlayerCharacter playerCharacter, FPPlayerBrain brain, FPPlayerAbilityManager manager)
        {
            base.Initialize(playerCharacter, brain, manager);
            SubscribeToInputEvents(brain);
            interactorSource = playerCharacter.camera.transform;
            rayActive = true;
        }

        /// <summary>
        /// I'm later going to see if I should carry this to PlayerAbility and subscribe to all events, OR do it from ability manager...
        /// </summary>
        public override void SubscribeToInputEvents(FPPlayerBrain brain)
        {
            base.SubscribeToInputEvents(brain);
            // For now Release only and Performed to simply interact
            //but over time I need to handle Hold, doubletap...etc
            brain.InteractAction.performed += InteractWithDetected;
            //brain.InteractAction.performed += InteractWithDetected;

            brain.InteractAction.Enable();
        }

        //private void UnSubscribeFromInputEvents()
        //{
        //    _brain.InteractAction.performed -= InteractWithDetected;
        //}

        protected override void Update()
        {
            base .Update();
            if (rayActive)
            {
                CastRayToDetectInteractable();
            }
        }
        private void CastRayToDetectInteractable()
        {
            Ray ray = new Ray(interactorSource.position, interactorSource.forward);
            Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.blue);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange))
            {
                Debug.Log("Raycast hit: " + hitInfo.collider.gameObject.name);

                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    Debug.Log("Interactable object detected: " + interactObj);

                    if (currentInteractable != interactObj)
                    {
                        if (currentInteractable != null)
                        {
                            currentInteractable.Highlight(false);
                        }

                        currentInteractable = interactObj;
                        currentInteractable.Highlight(true);

                    }
                }
                else
                {
                    Debug.Log("Raycast hit a non-interactable object: " + hitInfo.collider.gameObject.name);

                    if (currentInteractable != null)
                    {
                        currentInteractable.Highlight(false);
                        currentInteractable = null;
                    }
                }
            }
            else
            {
                Debug.Log("Raycast hit nothing.");

                if (currentInteractable != null)
                {
                    currentInteractable.Highlight(false);
                    currentInteractable = null;
                }
            }
        }

        private void InteractWithDetected(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                if (currentInteractable != null)
                {
                    //if (CanActivate() == true) // Debugging atm to see if canActivate works
                    if (canInteract)
                    {
                        currentInteractable.Interact();
                        Debug.Log($"Interacted with {currentInteractable.GetType().Name}");

                        //z999 Will bring here if equippable and how to handle if so
                    }
                    else
                    {
                        Debug.Log("Can't activate Interact ability / Can't interact");
                    }
                } 
            }
        }

        public override bool CanActivate()
        {
            //throw new System.NotImplementedException(); //temp
            /// Will firstly connect to the statemachine, health, consciousness or other states to drive it
            /// if above conditions, return false, else
            
            
            return true;
        }
        public override void Activate()
        {
            rayActive = true;
        }
        public override void Deactivate()
        {
            rayActive = false;
        }

        private void OnDestroy()
        {
            //UnSubscribeFromInputEvents();
        }

    }

}