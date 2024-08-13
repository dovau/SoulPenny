using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using UnityEngine.InputSystem;
using System.Linq;
using System.Xml;



namespace Soul
{
    public class FPBasicCharWTransitionSketch : MonoBehaviour
    {
        [SerializeField] private FPPlayerMediator mediator;
        [SerializeField] private FPPlayerBrain brain;

        [SerializeField] private AnimancerComponent animancer;

        //[SerializeField] private ClipTransition currentTransition;
        //[HideInInspector] public ClipTransition CurrentTransition => currentTransition;

        //[SerializeField] private ClipTransition idle;
        //[SerializeField] private ClipTransition walking;
        [SerializeField] private ClipTransitionAsset.UnShared action01;
        [SerializeField] private ClipTransitionAsset.UnShared jump;

        //[SerializeField] private LinearMixerTransitionAsset.UnShared _MixerMTA;
        [SerializeField] private MixerTransition2DAsset.UnShared GroundLocomotionM2D;
        [SerializeField] private MixerTransition2DAsset.UnShared CrouchedLocomotionM2D;
        [SerializeField] private ClipTransitionAsset.UnShared HandsIdle_CT;
        [SerializeField] private AnimancerTransitionAsset.UnShared Use_RH_01_ATA;

        [SerializeField] private AnimationSetBaseTest initialMoveSet;
        [SerializeField] private AnimationSetBaseTest currentWeaponAnimationSet;


        private Vector2 moveInputFromBrain;





        //Some temporary variables I'll be changing later
        private bool isSprinting = false; //this is temporary, will be replaced
        private bool isCrouching = false;
        [SerializeField] private bool altModifierOn = false;

        private AnimancerLayer baseLayer;
        private AnimancerLayer upperBody;
        private AnimancerLayer interactLayer;
        private AnimancerLayer interactLayerAlt;
        private AnimancerLayer socialLayer;

        [SerializeField] private AvatarMask baseMask;
        [SerializeField] private AvatarMask upperBodyMask;
        [SerializeField] private AvatarMask interactMaskMain;
        [SerializeField] private AvatarMask interactMaskOffHand;
        [SerializeField] private AvatarMask interactMaskBothHands;
        [SerializeField] private AvatarMask socialMask;

        //Temporary probably
        public bool rightHandDrawn = false;
        public bool leftHandDrawn = false;


        //Interaction animations to be replaced by animation set contents
        [SerializeField] private ClipTransitionAsset.UnShared idle;
        [SerializeField] private ClipTransitionAsset.UnShared useSingle;
        [SerializeField] private ClipTransitionAsset.UnShared useAuto;
        [SerializeField] private ClipTransitionAsset.UnShared primaryPrepare;
        [SerializeField] private ClipTransitionAsset.UnShared primarySingle;
        [SerializeField] private ClipTransitionAsset.UnShared primarySingle01;
        [SerializeField] private ClipTransitionAsset.UnShared primarySingle02;
        [SerializeField] private ClipTransitionAsset.UnShared primarySingle03;
        [SerializeField] private ClipTransitionAsset.UnShared primarySingle04;
        [SerializeField] private ClipTransitionAsset.UnShared primaryHeavyPrepare;
        [SerializeField] private ClipTransitionAsset.UnShared primaryHeavySingle;
        [SerializeField] private ClipTransitionAsset.UnShared primaryHeavyAuto;
        [SerializeField] private ClipTransitionAsset.UnShared primaryAuto;
        [SerializeField] private ClipTransitionAsset.UnShared altPrepare;
        [SerializeField] private ClipTransitionAsset.UnShared altSingle;
        [SerializeField] private ClipTransitionAsset.UnShared altAuto;
        [SerializeField] private ClipTransitionAsset.UnShared secondarySingle;
        [SerializeField] private ClipTransitionAsset.UnShared secondaryAuto;
        [SerializeField] private ClipTransitionAsset.UnShared reload;
        [SerializeField] private ClipTransitionAsset.UnShared throwPrepare;
        [SerializeField] private ClipTransitionAsset.UnShared throwSingle;
        [SerializeField] private ClipTransitionAsset.UnShared throwAuto;
        [SerializeField] private ClipTransitionAsset.UnShared inspectSingle;
        [SerializeField] private ClipTransitionAsset.UnShared inspectAuto;

        /// <summary>
        /// ToDo:
        /// 
        /// 1-Interact animations
        /// 
        /// 2- Jump animation
        /// 
        /// 
        /// one of the main problems is that since most events are handled outside
        /// the update loop, like walking and running, sprinting doesn't re-trigger 
        /// the animations as it should - since we STILL are getting the brain.movement.performed
        /// THUS I need to find a way to retrigger when sprint, jump, crouch...etc takes place.
        /// perhaps one way is to have an event like state changed, for classes like this to listen
        /// 
        /// 
        /// 
        /// 
        /// BetterAnimations
        /// 
        /// Crouch
        /// Slide
        /// Roll
        /// 
        /// Head IK
        /// 
        /// 
        /// States should pass their own mixers or animations
        /// I'm thinking of an event that says:
        /// OnItemChange 
        /// that looks for 
        /// OnEquip
        /// OnUnequip
        /// OnThrow
        /// OnDrop
        /// OnHolster
        /// OnDeplete
        /// OnDestroy...etc anything that changes that current item
        /// 
        /// Note to self, don't get attached to this idea.
        /// Basically:
        /// OnHandFull
        /// OnHandEmpty idk
        /// 
        /// There should be an equipment manager that keeps track of what is being held
        /// and what is being carried on person
        /// 
        /// Spells are also "equipped" in this context like skyrim perhaps
        /// 
        /// 
        /// </summary>





        private void Awake()
        {
            animancer.States.GetOrCreate(GroundLocomotionM2D);
            // Now the _Mixer.State will exist.

            InitializeLayers();
            SetLayerMasks();
            InitializeMoveSet();
            AssignAnimations();
            PlayInitialAnimations();
            ResetLayerWeights();



        }

        private void PlayInitialAnimations()
        {
            animancer.Play(GroundLocomotionM2D);
            interactLayer.Play(idle);
            interactLayerAlt.Play(idle);
            //Temporary fix below

        }
        private void InitializeMoveSet()
        {
            currentWeaponAnimationSet = initialMoveSet;
        }

        private void AssignAnimations()
        {
            idle = currentWeaponAnimationSet.idle;
            useSingle = currentWeaponAnimationSet.useSingle;
            useAuto = currentWeaponAnimationSet.useAuto;
            primaryPrepare = currentWeaponAnimationSet.primaryPrepare;
            primarySingle = currentWeaponAnimationSet.primarySingle;
            primarySingle01 = currentWeaponAnimationSet.primarySingle01;
            primarySingle02 = currentWeaponAnimationSet.primarySingle02;
            primarySingle03 = currentWeaponAnimationSet.primarySingle03;
            primarySingle04 = currentWeaponAnimationSet.primarySingle04;
            primaryAuto = currentWeaponAnimationSet.primaryAuto;
            primaryHeavyPrepare = currentWeaponAnimationSet.primaryHeavyPrepare;
            primaryHeavySingle = currentWeaponAnimationSet.primaryHeavySingle;
            primaryHeavyAuto = currentWeaponAnimationSet.primaryHeavyAuto;
            altPrepare = currentWeaponAnimationSet.altPrepare;
            altSingle = currentWeaponAnimationSet.altSingle;
            altAuto = currentWeaponAnimationSet.altAuto;
            secondarySingle = currentWeaponAnimationSet.secondarySingle;
            secondaryAuto = currentWeaponAnimationSet.secondaryAuto;
            reload = currentWeaponAnimationSet.reload;
            throwPrepare = currentWeaponAnimationSet.throwPrepare;
            throwSingle = currentWeaponAnimationSet.throwSingle;
            throwAuto = currentWeaponAnimationSet.throwAuto;
            inspectSingle = currentWeaponAnimationSet.inspectSingle;
            inspectAuto = currentWeaponAnimationSet.inspectAuto;
        }

        private void InitializeLayers()
        {
            baseLayer = animancer.Layers[0];
            upperBody = animancer.Layers[1];
            interactLayer = animancer.Layers[2]; //override
            interactLayerAlt = animancer.Layers[3]; // override
            socialLayer = animancer.Layers[4]; // additive?
        }

        private void SetLayerMasks()
        {
            baseLayer.SetMask(baseMask);
            upperBody.SetMask(upperBodyMask);
            interactLayer.SetMask(interactMaskMain); //maybe both hands later idk
            interactLayerAlt.SetMask(interactMaskOffHand);
            socialLayer.SetMask(socialMask);
        }

        private void ResetLayerWeights()
        {
            baseLayer.SetWeight(1);
            upperBody.SetWeight(0);
            interactLayer.SetWeight(0);
            interactLayerAlt.SetWeight(0);
            socialLayer.SetWeight(0);
        }


        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            UnSubscribeFromInputEvents();
        }

        //public float Speed
        //{
        //    get => _MixerMTA.State.Parameter;
        //    set => _MixerMTA.State.Parameter = value;
        //}

        private void Start()
        {
            if (brain != null)
            {
                brain.OnBrainInitialized += SubscribeToInputEvents;
            }
            //animancer.Play(idle, 0.25f);
        }


        private void SubscribeToInputEvents()
        {

            brain.Movement.performed += HandleLocomotionAnimation;
            brain.Movement.canceled += HandleLocomotionAnimation;

            brain.Jump.performed += HandleJumpAnimation;
            brain.Jump.canceled += HandleJumpAnimation;
            brain.Jump.started += HandleJumpAnimation;

            brain.Crouch.started += ToggleCrouchAnimation;
            //brain.Crouch.performed += HandleCrouchLocomotionAnimation;
            brain.Crouch.performed += ToggleCrouchAnimation;
            brain.Crouch.canceled += ToggleCrouchAnimation;


            brain.AltModifier.started += HandleAltModifier;
            brain.AltModifier.performed += HandleAltModifier;
            brain.AltModifier.canceled += HandleAltModifier;

            brain.Sprint.performed += HandleSprint;
            brain.Sprint.canceled += HandleSprint;

            brain.HolsterAction.performed += ToggleHolster; //Temporary 



            brain.InteractAction.performed += HandleInteractTransition;
            useSingle.Events.OnEnd = SetIdleRight; // temporary, just handles right hand and probably in a bad way

            brain.PrimaryAction.performed += HandlePrimaryAction;


            brain.Jump.Enable();
            brain.Movement.Enable();
            brain.Sprint.Enable();
            brain.Crouch.Enable();
            brain.InteractAction.Enable();
            brain.AltModifier.Enable();
            brain.PrimaryAction.Enable();
            //z999
        }

        private void UnSubscribeFromInputEvents()
        {
            brain.Movement.performed -= HandleLocomotionAnimation;
            brain.Movement.canceled -= HandleLocomotionAnimation;

            brain.Jump.performed -= HandleJumpAnimation;
            brain.Jump.canceled -= HandleJumpAnimation;
            brain.Jump.started -= HandleJumpAnimation;

            brain.Crouch.started -= ToggleCrouchAnimation;
            //brain.Crouch.performed -= HandleCrouchLocomotionAnimation;
            brain.Crouch.performed -= ToggleCrouchAnimation;
            brain.Crouch.canceled -= ToggleCrouchAnimation;

            brain.AltModifier.started -= HandleAltModifier;
            brain.AltModifier.performed -= HandleAltModifier;
            brain.AltModifier.canceled -= HandleAltModifier;

            brain.Sprint.performed -= HandleSprint;
            brain.Sprint.canceled -= HandleSprint;

            brain.HolsterAction.performed -= ToggleHolster; //Temporary 



            brain.InteractAction.performed -= HandleInteractTransition;
            useSingle.Events.OnEnd = null; // probably will mean set speed of locomotion mixer to 0 or sth

            brain.PrimaryAction.performed -= HandlePrimaryAction;


            brain.Jump.Disable();
            brain.Movement.Disable();
            brain.Sprint.Disable();
            brain.Crouch.Disable();
            brain.InteractAction.Disable();
            brain.AltModifier.Disable();
            brain.PrimaryAction.Disable();
        }
        private void ToggleHolster(InputAction.CallbackContext callbackContext)
        {

            // subject to change to better code
            ToggleRightHand();
            ToggleLeftHand();


        }

        private void ToggleRightHand()
        {
            if (!rightHandDrawn)
            {
                rightHandDrawn = true;
                interactLayer.SetWeight(1);
                //animancer.Layers[2].Play(); 
                //z9999  play the idle hands animation or then the mixer for interactions
            }
            else
            {
                rightHandDrawn = false;
                interactLayer.SetWeight(0);
            }

        }

        private void ToggleLeftHand()
        {
            if (!leftHandDrawn)
            {
                leftHandDrawn = true;
                interactLayerAlt.SetWeight(1);
            }
            else
            {
                leftHandDrawn = false;
                interactLayerAlt.SetWeight(0);
            }
        }

        private void HandleLocomotionAnimation(InputAction.CallbackContext context)
        {

            moveInputFromBrain = context.ReadValue<Vector2>();


            if (isSprinting)
            {
                moveInputFromBrain *= 2; // We're doubling the input just for now, for sprint
            }

            GroundLocomotionM2D.State.Parameter = moveInputFromBrain;

            if (isCrouching)
            {
                CrouchedLocomotionM2D.State.Parameter = moveInputFromBrain;
            }


        }

        private void ToggleCrouchAnimation(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isCrouching = true;
                animancer.Play(CrouchedLocomotionM2D);
            }
            else if (context.canceled)
            {
                isCrouching = false;
                animancer.Play(GroundLocomotionM2D);
            }

        }

        private void HandleJumpAnimation(InputAction.CallbackContext context)
        {
            if (context.canceled) { return; }
            if (context.started)
            {
                baseLayer.Play(jump);
            }


            else if (context.performed)
            {
                baseLayer.Play(GroundLocomotionM2D);
            }

        }

        private void HandleSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isSprinting = true;

            }
            else
            {
                isSprinting = false;
            }
            baseLayer.Play(GroundLocomotionM2D);

        }

        private void HandleAltModifier(InputAction.CallbackContext context)
        {
            //if (context.canceled) { return; }

            if (context.canceled)
            {
                altModifierOn = false;
                Debug.Log("AltModifier - Off");
            }

            else if (context.started)
            {
                altModifierOn = true;
                Debug.Log("AltModifier - On");
            }


        }


        private void SetIdleRight()
        {


            interactLayer.Play(idle);
            Debug.Log("Right hand set back to idle");
        }
        private void SetIdleLeft()
        {
            interactLayerAlt.Play(idle);
            Debug.Log("Left hand set back to idle");

        }

        private void HandleInteractTransition(InputAction.CallbackContext context)
        {
            if (altModifierOn)
            {
                interactLayer.Play(useSingle, 0.2f, FadeMode.FromStart);
            }
            else //z9999 Currently Right Hand only, will change soon 
            {
                {
                    interactLayerAlt.Play(useSingle, 0.2f, FadeMode.FromStart);

                }
            }

        }

        private void HandlePrimaryAction(InputAction.CallbackContext context)
        {
            interactLayer.Play(primarySingle);
            primarySingle.State.Events.OnEnd = SetIdleRight;

        }
    }

}