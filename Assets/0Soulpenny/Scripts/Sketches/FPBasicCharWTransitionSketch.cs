using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using UnityEngine.InputSystem;



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
        [SerializeField] private MixerTransition2DAsset.UnShared _Mixer2D;
        private Vector2 moveInputFromBrain;


        //Some temporary variables I'll be changing later
        private bool isSprinting = false; //this is temporary, will be replaced
        private bool altModifierOn = false;

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

        /// <summary>
        /// ToDo:
        /// 
        /// 1-Interact animations
        /// 
        /// 2- Jump animation
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
            animancer.States.GetOrCreate(_Mixer2D);
            // Now the _Mixer.State will exist.

            InitializeLayers();
            SetLayerMasks();
            ResetLayerWeights();

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
            animancer.Play(_Mixer2D);
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
            //brain.Movement.performed += HandleWalkTransition;
            //brain.Movement.canceled += HandleWalkTransition;

            brain.Movement.performed += HandleLocomotionAnimation;
            brain.Movement.canceled += HandleLocomotionAnimation;

            brain.Jump.performed += HandleJumpAnimation;
            brain.Jump.canceled += HandleJumpAnimation;
            brain.Jump.started += HandleJumpAnimation;

            brain.AltModifier.started += HandleAltModifier;
            brain.AltModifier.performed += HandleAltModifier;
            brain.AltModifier.canceled += HandleAltModifier;

            brain.Sprint.performed += HandleSprint;
            brain.Sprint.canceled += HandleSprint;



            brain.InteractAction.performed += HandleInteractTransition;
            action01.Events.OnEnd = OnActionEnd; // probably will mean set speed of locomotion mixer to 0 or sth



            brain.Jump.Enable();
            brain.Movement.Enable();
            brain.Sprint.Enable();
            brain.InteractAction.Enable();
            brain.AltModifier.Enable();
        }


        private void HandleLocomotionAnimation(InputAction.CallbackContext context)
        {
            moveInputFromBrain = context.ReadValue<Vector2>();

            if (isSprinting)
            {
                moveInputFromBrain *= 2; // We're doubling the input just for now, for sprint
            }

            _Mixer2D.State.Parameter = moveInputFromBrain;

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
                baseLayer.Play(_Mixer2D);
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
            //else if (context.performed)
            //{
            //    altModifierOn = true;
            //    Debug.Log("AltModifier - On");

            //}

        }


        private void OnActionEnd()
        {
            animancer.
            //ResetLayerWeights();
            //interactLayer.Stop();
            //interactLayerAlt.Stop();
        }
        private void HandleInteractTransition(InputAction.CallbackContext context)
        {
            if(altModifierOn)
            {
                interactLayer.Play(action01, 0.2f, FadeMode.FromStart);
            }
            else
            {
                {
                    interactLayerAlt.Play(action01, 0.2f, FadeMode.FromStart);
                }
            }
            //animancer.Play(action01, 0.25f, FadeMode.FromStart);
        }

        //private void HandleWalkTransition(InputAction.CallbackContext context)
        //{
        //    if (context.performed)
        //    {
        //        animancer.Play(walking, 0.25f);
        //    }
        //    else { animancer.Play(idle, 0.25f); }
        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }

}