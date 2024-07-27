using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM2;
using Animancer;
using UnityEngine.InputSystem;

namespace Soul
{

    public class FPPlayerMediator : CharacterMediator
    {
        private FPPlayerCharacter character;
        public FPPlayerCharacter Character { get { return character; } }
        
        private FPPlayerBrain brain;
        public FPPlayerBrain Brain { get { return brain; } }
        
        private FPPlayerAbilityManager abilityManager;
        public FPPlayerAbilityManager AbilityManager { get { return abilityManager; } }

        private AnimancerComponent animancer;
        public AnimancerComponent Animancer { get { return animancer; } }

        protected override void Awake()
        {
            base.Awake();
            character = GetComponent<FPPlayerCharacter>();
            if(character != null )
            {
                Debug.Log("Mediator found character: " + character.GetType().Name);
            }
            brain = GetComponentInChildren<FPPlayerBrain>();
            if( brain != null )
            {
                Debug.Log("Mediator found a brain: " + brain.GetType().Name);
            }
            abilityManager = GetComponentInChildren<FPPlayerAbilityManager>();

            if(abilityManager!= null )
            {
                Debug.Log("Mediator foudn an Ability Manager: "+ abilityManager.GetType().Name);
            }

            animancer = GetComponentInChildren<AnimancerComponent>();

            character.SetMediator(this);
            brain.SetMediator(this);
            abilityManager.SetMediator(this);
        }

        public override void Notify(object sender, string notifyEvent)
        {
            base.Notify(sender, notifyEvent);
            //later on this becomes override
            if (sender is FPPlayerCharacter characterSender)
            {
               if (characterSender == character && notifyEvent == "ActionPerformed")
                {
                    Debug.Log("YAAY!");
                } 
            }
            else if(sender is FPPlayerBrain brainSender)
            {
                if (brainSender == brain && notifyEvent == "BrainThinkingHmm")
                {
                    Debug.Log("Hmm...");
                }
            }
            else if(sender is FPPlayerAbilityManager abilityManagerSender)
            {
                if(abilityManagerSender == abilityManager && notifyEvent == "AbilityManagerNotifying")
                {
                    Debug.Log("Ability manager notification recieved");
                }
            }

        }


        
    }



}